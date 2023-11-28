using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Appcompras.Modelo;
using Appcompras.Datos;
using Appcompras.Vistas;
using Plugin.SharedTransitions;
using System.Linq;
namespace Appcompras.VistaModelo
{
    public class VMcompras : BaseViewModel
    {
        #region VARIABLES
        string _Texto;
        int _index;
        List<Mproductos> _listaproductos;
        List<Mdetallecompras> _listaVistapreviaDc;
        List<Mdetallecompras> _listaDc;
        bool _IsvisiblePaneldetallecompra;
        string _cantidadtotal;
        #endregion
        #region CONSTRUCTOR
        public VMcompras(INavigation navigation, StackLayout Carrilderecha, StackLayout Carrilizquierda)
        {
            Navigation = navigation;
            Mostrarproductos(Carrilderecha, Carrilizquierda);
            IsvisiblePanelDc = false;
        }
        #endregion

        #region OBJETOS
        public string  Cantidadtotal
        {
            get { return _cantidadtotal; }
            set { SetValue(ref _cantidadtotal, value); }
        }
        public bool IsvisiblePanelDc
        {
            get { return _IsvisiblePaneldetallecompra; }
            set { SetValue(ref _IsvisiblePaneldetallecompra, value); }
        }
        public List<Mdetallecompras> ListaVistapreviaDc
        {
            get { return _listaVistapreviaDc; }
            set { SetValue(ref _listaVistapreviaDc, value); }
        }
        public List<Mdetallecompras> ListaDc
        {
            get { return _listaDc; }
            set { SetValue(ref _listaDc, value); }
        }
        public List<Mproductos> Listaproductos
        {
            get { return _listaproductos; }
            set { SetValue(ref _listaproductos, value); }
        }

        private string _numeroTarjeta;
        public string NumeroTarjeta
        {
            get { return _numeroTarjeta; }
            set { SetValue(ref _numeroTarjeta, value); }
        }

        private string _nombreTitular;
        public string NombreTitular
        {
            get { return _nombreTitular; }
            set { SetValue(ref _nombreTitular, value); }
        }

        private string _fechaVencimiento;
        public string FechaVencimiento
        {
            get { return _fechaVencimiento; }
            set { SetValue(ref _fechaVencimiento, value); }
        }

        private string _codigoSeguridad;
        public string CodigoSeguridad
        {
            get { return _codigoSeguridad; }
            set { SetValue(ref _codigoSeguridad, value); }
        }

        private decimal _precioTotal;
        public decimal PrecioTotal
        {
            get { return _precioTotal; }
            set { SetValue(ref _precioTotal, value); }
        }
        #endregion
        #region PROCESOS
        public async Task Mostrarproductos(StackLayout Carrilderecha, StackLayout Carrilizquierda)
        {
            var funcion = new Dproductos();
            Listaproductos = await funcion.Mostrarproductos();
            var box = new BoxView
            {
                HeightRequest = 60
            };
            Carrilizquierda.Children.Clear();
            Carrilderecha.Children.Clear();
            Carrilderecha.Children.Add(box);
            foreach (var item in Listaproductos)
            {
                Dibujarproductos(item, _index, Carrilderecha, Carrilizquierda);
                _index++;
            }
        }
        public void Dibujarproductos(Mproductos item, int index, StackLayout Carriderecha, StackLayout Carrilizquierda)
        {
            var _ubicacion = Convert.ToBoolean(index % 2);
            var carril = _ubicacion ? Carriderecha : Carrilizquierda;

            var frame = new Frame
            {
                HeightRequest = 200,
                CornerRadius = 10,
                Margin = 8,
                HasShadow = false,
                BackgroundColor = Color.White,
                Padding = 30,
            };
            var stack = new StackLayout
            {

            };
            var image = new Image
            {
                Source = item.Icono,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 5)
            };
            var labelprecio = new Label
            {
                Text = "$" + item.Precio,
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                Margin = new Thickness(0, 6),
                TextColor = Color.FromHex("#333333")
            };
            var labeldescripcion = new Label
            {
                Text = item.Descripcion,
                FontSize = 16,
                TextColor = Color.Black,
                CharacterSpacing = 1
            };
            var labelpeso = new Label
            {
                Text = item.Peso,
                FontSize = 13,
                TextColor = Color.FromHex("#CCCCCC"),
                CharacterSpacing = 1
                ,Margin= new Thickness(0,0,0,5)
            };
            stack.Children.Add(image);
            stack.Children.Add(labelprecio);
            stack.Children.Add(labeldescripcion);
            stack.Children.Add(labelpeso);
            frame.Content = stack;
            var tap = new TapGestureRecognizer();
            tap.Tapped += async (object sender, EventArgs e) =>
             {
                 var page = (App.Current.MainPage as SharedTransitionNavigationPage).CurrentPage;
                 SharedTransitionNavigationPage.SetBackgroundAnimation(page, BackgroundAnimation.SlideFromRight);
                 SharedTransitionNavigationPage.SetTransitionDuration(page, 1000);
                 SharedTransitionNavigationPage.SetTransitionSelectedGroup(page, item.Idproducto);
                 await Navigation.PushAsync(new Agregarcompra(item));

             };

            carril.Children.Add(frame);
            stack.GestureRecognizers.Add(tap);
        }
        public async Task ProcesoAsyncrono()
        {

        }
        public void ProcesoSimple()
        {

        }
        public async Task MostrarVistapreviaDc()
        {
            var funcion = new Ddetallecompras();
            ListaVistapreviaDc = await funcion.MostrarVistapreviaDc();

            PrecioTotal = ListaVistapreviaDc.Sum(item =>
            {
                decimal total;
                if (decimal.TryParse(item.Total, out total))
                {
                    return total;
                }
                return 0; // Otra opción, dependiendo de cómo quieras manejar los errores de conversión
            });

            Cantidadtotal = (await funcion.Sumarcantidad()).ToString();
        }
        public async Task MostrarpanelDc(Grid gridproductos,StackLayout paneldetalleC,
           StackLayout panelcontador)
        {
            uint duracion = 700;
            await Task.WhenAll(
                panelcontador.FadeTo(0, 500),
                gridproductos.TranslateTo(0, -200, duracion+200, Easing.CubicIn),
                paneldetalleC.TranslateTo(0, -200, duracion, Easing.CubicIn)
                );
            IsvisiblePanelDc = true;
        }
        public async Task MostrargridProductos(Grid gridproductos, StackLayout paneldetalleC,
          StackLayout panelcontador)
        {
            uint duracion = 700;
            await Task.WhenAll(
                panelcontador.FadeTo(1, 500),
                gridproductos.TranslateTo(0, 0, duracion + 200, Easing.CubicIn),
                paneldetalleC.TranslateTo(0, 1000, duracion, Easing.CubicIn)
                );
            IsvisiblePanelDc = false;
        }
        public async Task MostrarDetalleC()
        {
            var funcion = new Ddetallecompras();
            ListaDc = await funcion.MostrarDc();
        }
        public async Task Sumarcantidades()
        {
            var funcion = new Ddetallecompras();
            Cantidadtotal = (await funcion.Sumarcantidad()).ToString();
        }


        public ICommand RealizarPagoCommand => new Command(async () => await RealizarPago());

        private async Task RealizarPago()
        {
            if (!EsNumeroTarjetaValido() || !EsNombreTitularValido() || !EsFechaVencimientoValida() || !EsCodigoSeguridadValido())
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, verifica los datos de la tarjeta.", "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert("Confirmación", "Gracias por tu compra.", "Aceptar");
        }

        private bool EsNumeroTarjetaValido()
        {
            return !string.IsNullOrWhiteSpace(NumeroTarjeta);
        }

        private bool EsNombreTitularValido()
        {
            return !string.IsNullOrWhiteSpace(NombreTitular);
        }

        private bool EsFechaVencimientoValida()
        {
            return !string.IsNullOrWhiteSpace(FechaVencimiento);
        }

        private bool EsCodigoSeguridadValido()
        {
            return !string.IsNullOrWhiteSpace(CodigoSeguridad);
        }

        public ICommand IrAPagoCommand => new Command(async () => await IrAPago());

        private async Task IrAPago()
        {
            await Navigation.PushAsync(new PagoConTarjeta());
        }

        
        #endregion
        #region COMANDOS
        public ICommand ProcesoAsyncommand => new Command(async () => await ProcesoAsyncrono());
        public ICommand ProcesoSimpcommand => new Command(ProcesoSimple);
        #endregion
    }
}
