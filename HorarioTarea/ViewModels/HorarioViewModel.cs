using GalaSoft.MvvmLight.Command;
using HorarioTarea.Models;
using HorarioTarea.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HorarioTarea.ViewModels
{
    public class HorarioViewModel : INotifyPropertyChanged
    {
        ClasesRepository clasesRepository = new();
        ActividadesRepository actividadesRepository = new();

        public object Seleccionado {  get; set; }
        public Clase Clase { get; set; }

        

        public Actividad Actividad { get; set; }

        //List<Clase> Clases { get; set; }

        //List<Actividad> Actividades { get; set; }


        //ObservableCollection<Clase> ListaClasesMostrar { get; set; }

        //ObservableCollection<Actividad> ListaActivdadesMostrar { get; set; }



        public ObservableCollection<Object> ListaDia { get; set; } = new();

        public void FiltrarDia()
        {
            ListaDia.Clear();
            foreach (var actividad in actividadesRepository.GetByDay(Dia))
            {
                ListaDia.Add(actividad);
            }

            foreach (var clase in clasesRepository.GetByDay(Dia))
            {
                ListaDia.Add(clase);
            }

            var Ordenados = ListaDia.OrderBy(x =>
            {
                if (x is Clase clase) return clase.HoraInicio;
                if (x is Actividad act) return act.HoraInicio;
                return Clase.HoraInicio;
            }
            ).ToList();


            ListaDia.Clear();

            foreach (var item in Ordenados)
            {
                ListaDia.Add(item);
            }



        }


        public string vista { get; set; } = "VerHorario";
        public ICommand AgregarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }

        public ICommand EliminarCommand { get; set; }

        public ICommand EditarCommand { get; set; }

        public ICommand ObtenerDiaCommand { get; set; }

        public string Error { get; set; }

        public string Dia { get; set; } = "Lunes";

        public HorarioViewModel() 
        {
            AgregarCommand = new RelayCommand<string>(Agregar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
            EliminarCommand = new RelayCommand(Eliminar);
            EditarCommand = new RelayCommand<string>(Editar);
            ObtenerDiaCommand = new RelayCommand<string>(ObtenerDia);
            FiltrarDia();
        }


        private bool TiempoOcupadoEnClases()
        {
            var clasesDelDia =clasesRepository.GetByDay(Clase.DiaSemena);
            foreach (var clase in clasesDelDia)
            {
                if (!(Clase.HoraFin <= clase.HoraInicio || Clase.HoraInicio >= clase.HoraFin))
                {
                    return false;
                }
            }
            return true;
        }

        private bool TiempoOcupadoEnActividad()
        {
            var ActividadDelDia = actividadesRepository.GetByDay(Actividad.DiaSemena);
            foreach (var actividad in ActividadDelDia)
            {
                if (!(Actividad.HoraFin <= actividad.HoraInicio || Actividad.HoraInicio >= actividad.HoraFin))
                {
                    return false;
                }
            }
            return true;
        }



        private void ObtenerDia(string dia)
        {
            Dia=dia;
            Actualizar(nameof(Dia));    
            FiltrarDia();
            

        }

        private void Editar(string Tipo)
        {
            Error = "";
           if(Tipo=="Clase")
            {
                if (Clase != null)
                {
                    if (string.IsNullOrWhiteSpace(Clase.nombre))
                    {
                        Error = "Ingrese su nombre";
                    }
                    if (string.IsNullOrWhiteSpace(Clase.Aula))
                    {
                        Error = "Ingrese su Aula";
                    }
                    if (string.IsNullOrWhiteSpace(Clase.maestro))
                    {
                        Error = "Ingrese su maestro";
                    }
                    if (Clase.HoraInicio < 0 || Clase.HoraInicio > 24)
                    {
                        Error = "Ingrese un formato de hora en 24 hrs";
                    }
                    if (Clase.HoraFin < 0 || Clase.HoraFin > 24)
                    {
                        Error = "Ingrese un formato de hora en 24 hrs";
                    }
                    if (Clase.HoraInicio > Clase.HoraFin)
                    {
                        Error = "La hora de inicio no puede ser mayor que la hora final";
                    }


                   

                    Actualizar(nameof(Error));

                    if (Error == "") 
                    { 
                    clasesRepository.Update(Clase);
                    CambiarVista("Cancelar");
                    FiltrarDia();
                    }
                }
            }
            if (Tipo == "Actividad")
            {

                if (Actividad != null)
                {
                    if (string.IsNullOrWhiteSpace(Actividad.Nombre))
                    {
                        Error = "Ingrese su nombre";
                    }

                    if (string.IsNullOrWhiteSpace(Actividad.Descripcion))
                    {
                        Error = "Ingrese su descripcion";
                    }
                    if (Actividad.HoraInicio < 0 || Actividad.HoraInicio > 24)
                    {
                        Error = "Ingrese un formato de hora en 24 hrs";
                    }
                    if (Actividad.HoraFin < 0 || Actividad.HoraFin > 24)
                    {
                        Error = "Ingrese un formato de hora en 24 hrs";
                    }
                    if (Actividad.HoraInicio > Actividad.HoraFin)
                    {
                        Error = "La hora de inicio no puede ser mayor que la hora final";
                    }


                    Actualizar(nameof(Error));

                    if (Error == "")
                    {

                        actividadesRepository.Update(Actividad);
                        CambiarVista("Cancelar");
                        FiltrarDia();

                    }
                }
            }

        }

        private void Eliminar()
        {
            if(Seleccionado is Clase)
            {
                
                clasesRepository.Delete(Clase);
                CambiarVista("Cancelar");
                Actualizar(nameof(vista));
                FiltrarDia();
            }
            else
            {
                actividadesRepository.Delete(Actividad);
                CambiarVista("Cancelar");
                Actualizar(nameof(vista)); 
                FiltrarDia();
            }
        }

        private void CambiarVista(string V)
        {
            if (V == "AgregarClase")
            {
                ListaDia.Clear();
                Clase = new();
                
                vista = "VerAgregarClase";
                Actualizar(nameof(Clase));
                
                Actualizar(nameof(vista));

            }
            else if (V=="AgregarActividad")
            {
                ListaDia.Clear();
               
                Actividad = new();
                vista = "VerAgregarActividad";
               
                Actualizar(nameof(Actividad));
                Actualizar(nameof(vista));
            }
            else if (V== "Cancelar")
            {
                vista = "VerHorario";
                Actualizar(nameof(vista));
                ListaDia.Clear();
                FiltrarDia();
            }
            else if (V == "VerEditar")
            {
                VericicarSeleccionado();
                if (Seleccionado != null && Seleccionado is Clase )
                {
                    if (Clase != null) 
                    { 
                         var clon = new Clase
                        {
                            Id=Clase.Id,
                            nombre=Clase.nombre,
                            maestro=Clase.maestro,
                            Aula=Clase.Aula,
                            DiaSemena=Clase.DiaSemena,
                            HoraFin=Clase.HoraFin,
                            HoraInicio=Clase.HoraInicio
                        
                        };

                        Clase = clon;
                        Actualizar (nameof(Actividad));
                        vista = "EditarClase";
                        Actualizar(nameof(vista));
                    }
                }
                else
                {
                    if (Actividad != null) 
                    { 
                        var clon = new Actividad
                        {
                            id = Actividad.id,
                            Nombre=Actividad.Nombre,
                            Descripcion = Actividad.Descripcion,
                            DiaSemena = Actividad.DiaSemena,
                            HoraFin = Actividad.HoraFin,
                            HoraInicio = Actividad.HoraInicio,
                        };
                        Actividad=clon;
                        Actualizar(nameof (Actividad));
                        vista = "EditarActividad";
                        Actualizar(nameof (vista));
                    }
                }
            }
            else if (V == "VerSeleccion")
            {
                vista = "Seleccion";
                Actualizar(nameof(vista));
            }
            else if (V == "VerEliminar")
            {
                VericicarSeleccionado();
                if(Seleccionado is Clase)
                {
                    if (Clase != null) 
                    { 
                        vista = "EliminarClase";
                        Actualizar(nameof(vista));
                    }
                }
                else
                {
                    if (Actividad != null) 
                    { 
                        vista = "EliminarActividad";
                        Actualizar(nameof(vista));
                    
                    }
                }
            }
        }

        private void Agregar(string Tipo)
        {
            Error = "";
            if (Tipo == "Clase")
            {
                
                if(Clase!=null)
                {
                    if (string.IsNullOrWhiteSpace(Clase.nombre))
                    {
                        Error = "Ingrese su nombre";
                    }
                    if (string.IsNullOrWhiteSpace(Clase.Aula))
                    {
                        Error = "Ingrese su Aula";
                    }
                    if (string.IsNullOrWhiteSpace(Clase.maestro))
                    {
                        Error = "Ingrese su maestro";
                    }
                   if(Clase.HoraInicio<0 || Clase.HoraInicio>24)
                    {
                        Error = "Ingrese un formato de hora en 24 hrs";
                    }
                    if (Clase.HoraFin < 0 || Clase.HoraFin > 24)
                    {
                        Error = "Ingrese un formato de hora en 24 hrs";
                    }
                    if(Clase.HoraInicio>Clase.HoraFin)
                    {
                        Error = "La hora de inicio no puede ser mayor que la hora final";
                    }


                    if (TiempoOcupadoEnClases() is false)
                    {
                        Error += "El espacio de tiempo esta ocupado";
                    }

                    Actualizar(nameof(Error));

                    if (Error == "") 
                    { 
                        clasesRepository.Insert(Clase);
                        CambiarVista("Cancelar");
                        FiltrarDia();
                    }
                
                }
            }
            else
            {
                if(Actividad!=null)
                {
                    if(string.IsNullOrWhiteSpace(Actividad.Nombre))
                    {
                        Error = "Ingrese su nombre";
                    }
                   
                    if (string.IsNullOrWhiteSpace(Actividad.Descripcion))
                    {
                        Error = "Ingrese su descripcion";
                    }
                    if (Actividad.HoraInicio < 0 || Actividad.HoraInicio > 24)
                    {
                        Error = "Ingrese un formato de hora en 24 hrs";
                    }
                    if (Actividad.HoraFin < 0 || Actividad.HoraFin > 24)
                    {
                        Error = "Ingrese un formato de hora en 24 hrs";
                    }
                    if (Actividad.HoraInicio > Actividad.HoraFin)
                    {
                        Error = "La hora de inicio no puede ser mayor que la hora final";
                    }


                    Actualizar(nameof(Error));

                    if (Error == "")
                    {
                        actividadesRepository.Insert(Actividad);
                        CambiarVista("Cancelar");
                        FiltrarDia();
                    }

                }

                
            }
        }

        void VericicarSeleccionado()
        {
            if(Seleccionado is Models.Clase)
            {
                Clase = new();
                Clase = (Clase)Seleccionado;
            }
            else
            {
                Actividad = new();
                Actividad = (Actividad)Seleccionado;
            }

        }
        void Actualizar(string? name = null)
        {
            PropertyChanged?.Invoke(this, new (name));  
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
