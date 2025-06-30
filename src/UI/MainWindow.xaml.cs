using Avalonia.Controls;
using GuardianDefinitivo.Data.Models; // Necesario para UserMembershipData
using System.Linq; // Para FirstOrDefault

namespace GuardianDefinitivo.UI
{
    public partial class MainWindow : Window
    {
        // Propiedad para almacenar y exponer los datos de membresía del usuario
        public UserMembershipData? UserProfile { get; }

        // Propiedad para facilitar el binding en XAML al nombre del guardián
        public string GuardianDisplayName { get; } = "Guardián Desconocido";

        // Constructor por defecto necesario para el diseñador de XAML y previsualizaciones
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            // Esto es útil para el diseñador de Avalonia en Rider o VS con plugin
            // Puedes crear datos de ejemplo aquí si es necesario para el diseño.
            // this.DataContext = this; // Si tienes propiedades directas en MainWindow para enlazar
#endif
        }

        // Constructor para ser llamado desde Program.cs con los datos del usuario
        public MainWindow(UserMembershipData userMembershipData)
        {
            InitializeComponent();
            UserProfile = userMembershipData;

            if (UserProfile?.DestinyMemberships != null && UserProfile.DestinyMemberships.Any())
            {
                // Intenta obtener el perfil primario o el primero de la lista
                var primaryMembership = UserProfile.DestinyMemberships.FirstOrDefault(m => m.MembershipId == UserProfile.PrimaryMembershipId)
                                        ?? UserProfile.DestinyMemberships.FirstOrDefault();

                if (primaryMembership != null)
                {
                    GuardianDisplayName = primaryMembership.DisplayName ?? primaryMembership.BungieGlobalDisplayName ?? "Guardián";
                }
            }
            else if (UserProfile?.BungieNetUser?.DisplayName != null)
            {
                 GuardianDisplayName = UserProfile.BungieNetUser.DisplayName;
            }

            this.DataContext = this; // Establece el DataContext para poder enlazar a GuardianDisplayName
        }
    }
}
