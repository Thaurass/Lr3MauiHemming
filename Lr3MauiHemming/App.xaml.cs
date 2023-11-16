namespace Lr3MauiHemming
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            const int newWidth = 700;
            const int newHeight = 560;

            window.Width = newWidth;
            window.Height = newHeight;

            window.MinimumWidth = 700;
            window.MaximumHeight = 560;

            return window;
        }
    }
}