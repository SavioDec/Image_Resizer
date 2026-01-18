using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace didaticos.redimensionador;

internal class Program {
    public static void Main(string[] args) {
        Console.WriteLine("Starting image resizer");


        Thread thread = new Thread(Redimensionar);
        thread.Start();
    }

    private static void Redimensionar() {
        #region Diretorios

        string diretorioEntrada = "Input_Files";
        string diretorioFinalizado = "Finished_Files";
        string diretorioRedimensionado = "Resized_Files";

        if (!Directory.Exists(diretorioEntrada)) {
            Directory.CreateDirectory(diretorioEntrada);
        }

        if (!Directory.Exists(diretorioFinalizado)) {
            Directory.CreateDirectory(diretorioFinalizado);
        }

        if (!Directory.Exists(diretorioRedimensionado)) {
            Directory.CreateDirectory(diretorioRedimensionado);
        }

        #endregion


        bool imagesSequence = true;
        while (imagesSequence) {
            try {
                //Programa olha para a pasta de entrada
                // Se tiver arquivo, ele ira redimensionar
                var arquivosEntrada = Directory.EnumerateFiles(diretorioEntrada);
                if (arquivosEntrada.Count() == 0) {
                    Console.WriteLine("Put the files in the Input_Files directory");
                }

                Console.Write("Enter the desired new height: ");
                var textoDigitado = Console.ReadLine();

                //Trata Erros de input
                if (!int.TryParse(textoDigitado, out int novaAltura)) throw new Exception($"The value '{textoDigitado}' needs to be an integer");
                if (novaAltura <= 0) throw new Exception($"The value '{novaAltura}' cannot be negative");

                if (arquivosEntrada.Count() == 0) {
                    imagesSequence = false;
                }

                // ler o tamanho que ira redimensionar

                foreach (var arquivo in arquivosEntrada) {
                    FileStream fileStream =
                        new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    FileInfo fileInfo = new FileInfo(arquivo);

                    string extensao = fileInfo.Extension;
                    string nomeSemExtensao = Path.GetFileNameWithoutExtension(arquivo);
                    string novoNome = $"{nomeSemExtensao}_{DateTime.Now:dd_MM_yy_HHmmss}{extensao}";

                    string caminho = Path.Combine(Environment.CurrentDirectory, diretorioRedimensionado, novoNome);

                    //Redimensiona & Copia os arquivos redimensionados para a pasta de redimensionados
                    Redimensionador(imagem: Image.Load(fileStream), novaAltura, caminho);

                    //Fecha o arquivo
                    fileStream.Close();


                    //move arquivo de entrada para a pasta de finalizados
                    string caminhoFinalizado =
                        Path.Combine(Environment.CurrentDirectory, diretorioFinalizado, fileInfo.Name);
                    fileInfo.MoveTo(caminhoFinalizado);
                }

                Thread.Sleep(new TimeSpan(0, 0, 1));


                Console.WriteLine($"All files resized!");
            }
            catch (Exception e) {
                Console.Clear();
                Console.WriteLine($"Error: {e.Message}\nPlease enter a valid number");
            }
        }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="imagem">Imagem a ser redimensionada</param>
    /// <param name="altura">Altura que desejamos redimensionar</param>
    /// <param name="caminho">Caminho onde iremos gravar o arquivo redimensionado</param>
    /// <returns></returns>
    static void Redimensionador(Image imagem, int altura, string caminho) {
        double ratio = (double)altura / imagem.Height;
        int novaLargura = (int)(imagem.Width * ratio);
        int novaAltura = (int)(imagem.Height * ratio);

        imagem.Mutate(x => x.Resize(novaLargura, novaAltura));

        imagem.Save(caminho);
        imagem.Dispose();
    }
}