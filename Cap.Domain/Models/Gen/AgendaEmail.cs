﻿using Cap.Domain.Models.Admin;
using Cap.Domain.Service.Admin;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cap.Domain.Models.Gen
{
    public class AgendaEmail
    {
        [Key]
        public int Id { get; set; }

        [Range(1,double.MaxValue, ErrorMessage ="Nenhum vínculo com algum contato da agenda")]
        public int IdAgenda { get; set; }

        [Required(ErrorMessage ="Informe o email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Contato { get; set; }

        [Required]
        [Display(Name = "Alterado por")]
        public int AlteradoPor { get; set; }

        [Required]
        [Display(Name = "Alterado em")]
        public DateTime AlteradoEm { get; set; }

        [NotMapped]
        [Display(Name = "Usuário")]
        public virtual Usuario Usuario
        {
            get
            {
                return new UsuarioService().Find(AlteradoPor);
            }
            set { }
        }
    }
}