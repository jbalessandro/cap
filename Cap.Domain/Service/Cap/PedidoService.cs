﻿using Cap.Domain.Abstract;
using Cap.Domain.Abstract.Cap;
using Cap.Domain.Models.Cap;
using Cap.Domain.Respository;
using System;
using System.Linq;

namespace Cap.Domain.Service.Cap
{
    public class PedidoService : IBaseService<Pedido>
    {
        private IBaseRepository<Pedido> repository;
        private IBaseService<Parcela> serviceParcela;

        public PedidoService()
        {
            repository = new EFRepository<Pedido>();
            serviceParcela = new ParcelaService();
        }

        public Pedido Excluir(int id)
        {
            var pedido = repository.Find(id);

            if (pedido == null)
            {
                throw new ArgumentException("Pedido inexistente");
            }

            var parcelasEmAberto = pedido.Parcelas.Where(x => x.IdFpgto == null).ToList();

            foreach (var item in parcelasEmAberto)
            {
                serviceParcela.Excluir(item.Id);
            }

            if (pedido.Parcelas.Count == 0)
            {
                try
                {
                    return repository.Excluir(id);
                }
                catch (Exception)
                {
                    pedido.Ativo = false;
                    pedido.AlteradoEm = DateTime.Now;
                    return repository.Alterar(pedido);
                }
            }
            return pedido;
        }

        public Pedido Find(int id)
        {
            return repository.Find(id);
        }

        public int Gravar(Pedido item)
        {
            item.AlteradoEm = DateTime.Now;
            item.DataNF = item.DataNF == DateTime.MinValue ? null : item.DataNF;
            item.NF = (item.NF == null ? string.Empty : item.NF.ToUpper().Trim());
            if (string.IsNullOrEmpty(item.NF) && item.DataNF == DateTime.MinValue)
            {
                throw new ArgumentException("Informe a data de emissão da Nota Fiscal");
            }

            if (item.NF.Length > 0)
            {
                var pedido = repository.Listar().Where(x => x.IdEmpresa == item.IdEmpresa && x.NF == item.NF && x.IdFornecedor == item.IdFornecedor && x.Id != item.Id).FirstOrDefault();
                if (pedido != null)
                {
                    throw new ArgumentException($"Já existe uma nota fiscal com este número lançada para este fornecedor no pedido {pedido.Id}");
                }                
            }

            if (item.Id == 0)
            {
                item.CriadoEm = DateTime.Now;
                item.Ativo = true;
                return repository.Incluir(item).Id;
            }

            return repository.Alterar(item).Id;
        }

        public IQueryable<Pedido> Listar()
        {
            return repository.Listar();
        }
    }
}
