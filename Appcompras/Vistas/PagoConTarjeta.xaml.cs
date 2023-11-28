using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Appcompras.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagoConTarjeta : ContentPage
    {
        public PagoConTarjeta()
        {
            InitializeComponent();
        }

        private async void Button_Clicked_Registro(object sender, EventArgs e)
        {
            // Verificaciones de tipo para los datos del formulario
            if (string.IsNullOrWhiteSpace(NumeroTarjetaEntry.Text) || !EsNumeroTarjetaValido(NumeroTarjetaEntry.Text))
            {
                DisplayAlert("Error", "Por favor, ingrese un número de tarjeta válido.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(NombreTitularEntry.Text))
            {
                DisplayAlert("Error", "Por favor, ingrese el nombre del titular de la tarjeta.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(FechaVencimientoEntry.Text) || !EsFechaVencimientoValida(FechaVencimientoEntry.Text))
            {
                DisplayAlert("Error", "Por favor, ingrese una fecha de vencimiento válida (MM/YY).", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(CodigoSeguridadEntry.Text) || !EsCodigoSeguridadValido(CodigoSeguridadEntry.Text))
            {
                DisplayAlert("Error", "Por favor, ingrese un código de seguridad válido.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(NombreCompletoEntry.Text))
            {
                DisplayAlert("Error", "Por favor, ingrese su nombre completo.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(DireccionEntry.Text))
            {
                DisplayAlert("Error", "Por favor, ingrese su dirección.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(CiudadEntry.Text))
            {
                DisplayAlert("Error", "Por favor, ingrese su ciudad.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(CodigoPostalEntry.Text))
            {
                DisplayAlert("Error", "Por favor, ingrese su código postal.", "OK");
                return;
            }


            DisplayAlert("¡Gracias por tu compra!", "Agradecemos tu preferencia. ¡Vuelve pronto!", "Aceptar");

            await Navigation.PushAsync(new LoginPage());
        }

        private bool EsNumeroTarjetaValido(string numeroTarjeta)
        {
            return !string.IsNullOrWhiteSpace(numeroTarjeta) && numeroTarjeta.Length == 16;
        }

        private bool EsFechaVencimientoValida(string fechaVencimiento)
        {
            return !string.IsNullOrWhiteSpace(fechaVencimiento) && fechaVencimiento.Length == 5;
        }

        private bool EsCodigoSeguridadValido(string codigoSeguridad)
        {
            return !string.IsNullOrWhiteSpace(codigoSeguridad) && codigoSeguridad.Length == 3;
        }
    }
}