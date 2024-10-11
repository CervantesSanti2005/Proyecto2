using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Threading.Tasks;

namespace BDMusica{
    public partial class MainWindow : Window{
        private BaseDeDatos db;
        private Minero minero;

        public MainWindow(){
            InitializeComponent();

            try{
                // Inicializar la base de datos y el minero
                db = new BaseDeDatos();
                db.Database("musica-db.db");
                db.crearTabla();
                db.insertarDefault();
                minero = new Minero(db);
            }catch (Exception ex){

                MostrarMensaje($"Error inicializando la base de datos o el minero: {ex.Message}");
            }
        }

        private async void MineButton_Click(object sender, RoutedEventArgs e){
            string directorio = DirectoryTextBox.Text;  // Obtener la ruta del directorio ingresado

            if (!string.IsNullOrEmpty(directorio)){
                try{
                    // Confirmar si el directorio existe
                    if (!System.IO.Directory.Exists(directorio)){
                        MostrarMensaje("El directorio especificado no existe.");
                        return;
                    }
                    List <string> resultadosMinado = null;

                    await Task.Run(() => {
                        try{
                            resultadosMinado = minero.minarDirectorio(directorio);  // Minar archivos en el directorio
                        }catch (Exception minarEx){
                            MostrarMensaje($"Error durante el proceso de minado: {minarEx.Message}");
                            Console.WriteLine($"Error durante el minado: {minarEx}");
                        }
                    });
                    ResultsListView.ItemsSource = resultadosMinado;

                    MostrarMensaje("El proceso de minado ha terminado exitosamente.");
                }catch (Exception ex){
                    MostrarMensaje($"Error general en el proceso de minado: {ex.Message}");
                    Console.WriteLine($"Error general: {ex}");
                }
            }else{
                MostrarMensaje("Por favor, ingrese una ruta de directorio válida.");
            }
        }

        // Método auxiliar para mostrar mensajes en la interfaz
        private void MostrarMensaje(string mensaje){
            // Puedes tener un TextBlock en la interfaz para mostrar mensajes
            // Si no tienes un TextBlock, puedes agregar uno
            MensajeTextBlock.Text = mensaje;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e){
            string consulta = QueryTextBox.Text;

            if(!string.IsNullOrEmpty(consulta)){
                try{
                    ResultsListView.ItemsSource = null;
                    var busqueda = new Busqueda(db);

                    List<string> resultadosBusqueda = busqueda.Buscar(consulta);
                    if(resultadosBusqueda.Count > 0){
                        ResultsListView.ItemsSource = resultadosBusqueda;
                    }else{
                        MostrarMensaje("No se encontraron resultados");
                    }
                }catch{
                MostrarMensaje($"Error durante la búsqueda");
                Console.WriteLine($"Error en la búsqueda");
                }
            }else{
                MostrarMensaje("Consulta invalida");
            }
        }
    }
}
