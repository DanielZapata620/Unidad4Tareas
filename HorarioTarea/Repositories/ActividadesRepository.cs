using HorarioTarea.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorarioTarea.Repositories
{
    public class ActividadesRepository
    {
        SQLiteConnection conexion;
        public ActividadesRepository()
        {
            string ruta = "notas.sqlite";
            conexion = new(ruta);
            conexion.CreateTable<Actividad>();
        }
        public IEnumerable<Actividad> GetAll()
        {
            return conexion.Table<Actividad>().OrderBy(x => x.Descripcion);
        }

        public Actividad? Get(int id)
        {
            return conexion.Get<Actividad>(id);
        }

        public IEnumerable<Actividad> GetByDay(string day)
        {
            return conexion.Table<Actividad>().Where(x => x.DiaSemena == day);
        }

        public void Insert(Actividad activiidad)
        {
            conexion.Insert(activiidad);
        }

        public void Update(Actividad activiidad)
        {
            conexion.Update(activiidad);
        }
        public void Delete(Actividad activiidad)
        {

            conexion.Delete(activiidad);
        }

    }
}
