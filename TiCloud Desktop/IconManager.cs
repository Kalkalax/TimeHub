using System.IO;

namespace TiCloud
{
    /// <summary>
    /// Klasa odpowiedzialna za zarządzanie ikoną aplikacji w zasobniku systemowym (tray) oraz jej interakcjami.
    /// Tworzy i konfiguruje ikonę aplikacji, a także zapewnia jej odpowiednią obsługę, w tym widoczność i usuwanie.
    /// </summary>
    public class IconManager : IDisposable
    {
        /// <summary>
        /// Ścieżka do pliku ikony aplikacji, który będzie wyświetlany w zasobniku systemowym.
        /// </summary>
        public static readonly string TiCloudIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "TiCloud_eGp_icon.ico");

        /// <summary>
        /// Ikona aplikacji wyświetlana w zasobniku systemowym (tray), która reprezentuje stan aplikacji.
        /// </summary>
        private readonly NotifyIcon _notifyIcon;

        /// <summary>
        /// Tworzy nową instancję <see cref="IconManager"/>, konfiguruje ikonę w zasobniku systemowym oraz jej akcje.
        /// </summary>
        /// <param name="onOpen">Akcja do wykonania po kliknięciu na ikonę (np. przywrócenie okna).</param>
        /// <param name="onExit">Akcja do wykonania po wybraniu opcji "Exit" z menu kontekstowego (np. zakończenie aplikacji).</param>
        public IconManager(Action onOpen, Action onExit)
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon(TiCloudIconPath),
                Visible = true,
                Text = "TiCloud - Kliknij, aby przywrócić"
            };

            //// Tworzenie menu kontekstowego
            //var contextMenu = new ContextMenuStrip();
            //contextMenu.Items.Add("Open", null, (sender, args) => onOpen?.Invoke());
            //contextMenu.Items.Add("Exit", null, (sender, args) => onExit?.Invoke());
            //_notifyIcon.ContextMenuStrip = contextMenu;

            //// Obsługa zdarzeń
            //_notifyIcon.DoubleClick += (sender, args) => onOpen?.Invoke();
        }

        /// <summary>
        /// Zwalnia zasoby zajmowane przez obiekt <see cref="IconManager"/>, w tym usuwa ikonę z zasobnika systemowego.
        /// </summary>
        public void Dispose()
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
        }
    }
}
