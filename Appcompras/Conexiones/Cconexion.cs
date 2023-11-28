using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;
namespace Appcompras.Conexiones
{
   public class Cconexion
    {
        public static FirebaseClient firebase = new FirebaseClient("https://appcompras-28f09-default-rtdb.firebaseio.com/");

        public const string WepApyAuthentication = "AIzaSyA5iL7WXim-oYZy1Dn4VdFn4qgji8AQCHo";
    }
}
