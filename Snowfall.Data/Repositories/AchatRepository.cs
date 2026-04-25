using Dapper;
using Snowfall.Data.Context;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public class AchatRepository : IAchatRepository
{
    private readonly DapperContext _dbContext;

    public AchatRepository(DapperContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Achat> Create(Achat achat)
    {
        using var connection = _dbContext.CreateConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            var sqlAchat = @"
                INSERT INTO achats (utilisateur_id, sous_total, livraison, total, created_at, statut_paiement, stripe_session_id)
                VALUES (@UtilisateurId, @SousTotal, @Livraison, @Total, @CreatedAt, @StatutPaiement, @StripeSessionId)
                RETURNING id";

            achat.CreatedAt = DateTime.Now;
            achat.Id = await connection.QuerySingleAsync<int>(sqlAchat, achat, transaction: transaction);

            foreach (var ligne in achat.LignesAchat ?? [])
            {
                ligne.AchatId = achat.Id;
                var sqlLigne = @"
                    INSERT INTO lignes_achat (achat_id, evenement_id, quantite, prix_unitaire, evenement_nom)
                    VALUES (@AchatId, @EvenementId, @Quantite, @PrixUnitaire, @EvenementNom)
                    RETURNING id";
                ligne.Id = await connection.QuerySingleAsync<int>(sqlLigne, ligne, transaction: transaction);
            }

            transaction.Commit();
            return achat;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<Achat?> FindById(int id)
    {
        using var connection = _dbContext.CreateConnection();

        var sql = @"
            SELECT a.*, l.*, e.*
            FROM achats a
            LEFT JOIN lignes_achat l ON l.achat_id = a.id
            LEFT JOIN evenements e ON e.id = l.evenement_id
            WHERE a.id = @Id";

        var achatDict = new Dictionary<int, Achat>();

        await connection.QueryAsync<Achat, LigneAchat, Evenement, Achat>(
            sql,
            (achat, ligne, evenement) =>
            {
                if (!achatDict.TryGetValue(achat.Id, out var existingAchat))
                {
                    existingAchat = achat;
                    existingAchat.LignesAchat = new List<LigneAchat>();
                    achatDict[achat.Id] = existingAchat;
                }

                if (ligne != null)
                {
                    ligne.Evenement = evenement;
                    existingAchat.LignesAchat!.Add(ligne);
                }

                return existingAchat;
            },
            new { Id = id },
            splitOn: "id,id"
        );

        return achatDict.Values.FirstOrDefault();
    }
    
    public async Task<Achat?> FindStripeSessionId(string sessionId)
    {
        using var connection = _dbContext.CreateConnection();

        var sql = @"
            SELECT *
            FROM achats
            WHERE stripe_session_id = @SessionId";
        
        return await connection.QuerySingleOrDefaultAsync<Achat>(
            sql, new{ SessionId = sessionId });
    }
    
    public async Task<bool> MarquerCommePayer(int id, string paymentIntentId)
    {
        using var connection = _dbContext.CreateConnection();

        var sql = @"
            UPDATE achats SET
                stripe_payment_intent_id = @PaymentIntentId,
                statut_paiement =  @StatutPaiement
            WHERE id = @Id
            ";
        
        var affectedRows = await connection.ExecuteAsync(sql, new {Id = id, PaymentIntentId = paymentIntentId, StatutPaiement = StatutPaiement.Paye});
        return affectedRows == 1;
    }
    
    public async Task<bool> MarquerCommeAnnuler(int id)
    {
        using var connection = _dbContext.CreateConnection();

        var sql = @"
            UPDATE achats SET
                statut_paiement =  @StatutPaiement
            WHERE id = @Id
            ";
        
        var affectedRows = await connection.ExecuteAsync(sql, new {Id = id, StatutPaiement = StatutPaiement.Annule});
        return affectedRows == 1;
    }
}

