using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;

namespace BDMusica{
    public partial class MainWindow : Window{
        public MainWindow(){
            InitializeComponent();
            Console.WriteLine("Ventana MainWindow inicializada");
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e){
            string consulta = QueryTextBox.Text;  // Recoge la entrada del usuario

            if (!string.IsNullOrEmpty(consulta)){
                // Llamar al método que busca canciones y asignar los resultados
                var resultados = BuscarCanciones(consulta);

                // Usar ItemsSource para actualizar los ítems del ListBox
                ResultsListView.ItemsSource = resultados;
            }
        }

        // Este método devuelve una lista de canciones simulada
        private List<string> BuscarCanciones(string consultaUsuario){
            // Aquí iría la lógica de búsqueda real, por ahora simulamos con datos estáticos
            var resultados = new List<string>{
                "Canción 1 - Artista 1",
                "Canción 2 - Artista 2"
            };

            return resultados;
        }
    }
}
