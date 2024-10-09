using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace BDMusica{
    public partial class App : Application{
        public override void Initialize(){
            AvaloniaXamlLoader.Load(this);  // Carga el archivo App.axaml
        }

        public override void OnFrameworkInitializationCompleted(){
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop){
                desktop.MainWindow = new MainWindow();  // Carga MainWindow como ventana principal
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
