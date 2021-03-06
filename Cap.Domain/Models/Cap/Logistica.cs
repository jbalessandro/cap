﻿using Cap.Domain.Models.Admin;
using Cap.Domain.Service.Admin;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cap.Domain.Models.Cap
{
    public class Logistica
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Empresa")]
        [Range(1, double.MaxValue, ErrorMessage = "Empresa inválida")]
        public int EmpresaId { get; set; }

        [Display(Name = "Motorista")]
        [Range(1, double.MaxValue, ErrorMessage = "Selecione o motorista")]
        public int MotoristaId { get; set; }

        [Display(Name = "Data")]
        [Required(ErrorMessage = "Informe a data do serviço")]
        public DateTime DataServico { get; set; }

        [Display(Name = "Serviço")]
        [Required(ErrorMessage = "Informe o serviço")]
        [DataType(DataType.MultilineText)]
        public string Servico { get; set; }

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string Observ { get; set; }

        [Display(Name = "Alterado por")]
        [Required(ErrorMessage = "Usuário inválido")]
        public int UsuarioId { get; set; }

        [Display(Name = "Alterado em")]
        [Required]
        public DateTime AlteradoEm { get; set; }

        public bool Ativo { get; set; }

        [Display(Name = "Serviço concluído?")]
        public bool Concluido { get; set; }

        [Display(Name = "Concluído em")]
        public DateTime? ConcluidoEm { get; set; }

        [Display(Name = "Concluído por")]
        public int? ConcluidoPor { get; set; }

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string ConcluidoObserv { get; set; }

        public virtual Motorista Motorista { get; set; }

        public virtual Empresa Empresa { get; set; }
        
        public virtual Usuario Usuario { get; set; }

        public virtual string ConcluidoPorUsuario
        {
            get
            {
                if (ConcluidoPor == null)
                {
                    return string.Empty;
                }
                return new UsuarioService().Find((int)ConcluidoPor).Nome;
            }
        }
    }
}
