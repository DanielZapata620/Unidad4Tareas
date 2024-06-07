using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorarioTarea.Models
{
    [Table("Actividades")]
    public class Actividad
    {
        [PrimaryKey,AutoIncrement]
        public int id {  get; set; }

        [NotNull, MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [NotNull,MaxLength(250)]
        public string Descripcion { get; set; } = null!;

        [NotNull]
        public int HoraInicio { get; set; }

        [NotNull]
        public int HoraFin { get; set; }

        public string DiaSemena { get; set; }=null!;
        public string Tipo { get; set; } = "Actividad";

    }
}
