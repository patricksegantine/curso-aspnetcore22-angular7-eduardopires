using Dapper;
using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventos.IO.Infra.Data.Repository
{
    public class EventoRepository : Repository<Evento>, IEventoRepository
    {
        public EventoRepository(EventosContext context)
            : base(context)
        {

        }

        public IEnumerable<Categoria> ObterCategorias()
        {
            var sql = @"SELECT * FROM Categorias";

            return Db.Database.GetDbConnection().Query<Categoria>(sql);
        }

        public override Evento ObterPorId(Guid id)
        {
            // EF Core: Retorna o evento com endereço
            //return Db.Eventos.Include(e => e.Endereco).FirstOrDefault(e => e.Id == id);

            var sql = "SELECT * FROM Eventos e " +
                      "LEFT JOIN Enderecos en ON e.Id = en.EventoId " +
                      "WHERE e.Id = @uid";
            
            var evento = Db.Database.GetDbConnection().Query<Evento, Endereco, Evento>(sql,
                (e, en) =>
                {
                    if (en != null)
                        e.AtribuirEndereco(en);

                    return e;
                }, new { uid = id });

            return evento.FirstOrDefault();
        }

        public override IEnumerable<Evento> ObterTodos()
        {
            var sql = "SELECT * FROM Eventos e " +
                      "WHERE e.Excluido = 0 " +
                      "ORDER BY e.DataFim DESC";

            return Db.Database.GetDbConnection().Query<Evento>(sql);
        }

        public IEnumerable<Evento> ObterEventoPorOrganizador(Guid organizadorId)
        {
            // EF Core
            // return Db.Eventos.Where(e => e.OrganizadorId == organizadorId);

            // Dapper
            var sql = "SELECT * FROM Eventos e " +
                      "WHERE e.Excluido = 0 " +
                      "AND e.OrganizadorId = @oid " +
                      "ORDER BY e.DataFim DESC";

            return Db.Database.GetDbConnection().Query<Evento>(sql, new { oid = organizadorId });
        }

        public override void Excluir(Guid id)
        {
            var evento = ObterPorId(id);
            evento.ExcluirEvento(); // Marca o registro para exclusão lógica
            Atualizar(evento); // Invoca o método de atualização para evitar que qualquer aplicação exlui o registro no BD
        }

        #region Endereço

        public void IncluirEndereco(Endereco endereco)
        {
            Db.Enderecos.Add(endereco);
        }

        public void AtualizarEndereco(Endereco endereco)
        {
            Db.Enderecos.Update(endereco);
        }

        public Endereco ObterEnderecoPorId(Guid id)
        {
            //return Db.Enderecos.Find(id);

            var sql = "SELECT * FROM Enderecos e " +
                      "WHERE e.Id = @uid";

            var endereco = Db.Database.GetDbConnection().Query<Endereco>(sql, new { uid = id });

            return endereco.SingleOrDefault();
        }

        #endregion

    }
}
