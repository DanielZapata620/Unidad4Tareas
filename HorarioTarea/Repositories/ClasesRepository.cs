using HorarioTarea.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorarioTarea.Repositories
{
    public class ClasesRepository
    {
        SQLiteConnection conexion;
        public ClasesRepository()
        {
            string ruta ="notas.sqlite";
            conexion = new(ruta);
            conexion.CreateTable<Clase>();
        }
        public IEnumerable<Clase> GetAll()
        {
            return conexion.Table<Clase>().OrderBy(x => x.nombre);
        }
        public IEnumerable<Clase> GetByDay(string day)
        {
            return conexion.Table<Clase>().Where(x => x.DiaSemena == day);
        }

        public Clase? Get(int id)
        {
            return conexion.Get<Clase>(id);
        }

        public void Insert(Clase clase)
        {
            conexion.Insert(clase);
        }

        public void Update(Clase clase)
        {
            conexion.Update(clase);
        }
        public void Delete(Clase clase)
        {

            conexion.Delete(clase);
        }

    }
}
