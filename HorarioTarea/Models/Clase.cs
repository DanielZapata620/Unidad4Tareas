using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorarioTarea.Models
{
    [Table("Clases")]
    public class Clase
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [NotNull,MaxLength(50)]
        public string nombre { get; set; } = null!;
        [MaxLength(50)]
        public string maestro { get; set; }=null!;
        [NotNull,MaxLength(20)]
        public string Aula { get; set; } = null!;
        [NotNull]
        public int HoraInicio { get; set; }
        [NotNull]
        public int HoraFin {  get; set; }

        public string DiaSemena { get; set; } = null!;
        public string Tipo { get; set; } = "Clase";


    }
}
