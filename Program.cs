using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace didaticos.redimensionador;

internal class Program {
    public static void Main(string[] args) {
        Console.WriteLine("Starting image resizer");

        List<int> altura = PegarListaAltura();

        Redimensionar(altura);
    }

    private static List<int> PegarListaAltura() {
        List<int> listaAlturas = new List<int>();
        int quantidade = LerInteiro("How many Diferent Heights do you want? ");

        for (int i = 1; i <= quantidade; i++) {
            int altura = LerInteiro($"Enter height #{i} (in pixels)");
            listaAlturas.Add(altura);
        }

        return listaAlturas;
    }

    private static int LerInteiro(string texto) {
        while (true) {
            Console.Write(texto);
            string input = Console.ReadLine();

            if (int.TryParse(input, out int valor) && valor > 0) return valor;

            Console.WriteLine("Invalid input. Please enter a positive integer greater than zero.");
        }
    }

    private static void Redimensionar(List<int> listaAltura) {
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

        while (Directory.GetFiles(diretorioEntrada).Length == 0) {
            Console.Clear();
            Console.WriteLine("Put the files in the Input_Files directory");

            Thread.Sleep(2000);
        }

        FileStream fileStream;
        FileInfo fileInfo;

        bool imagesSequence = true;
        while (imagesSequence) {
            //Programa olha para a pasta de entrada
            // Se tiver arquivo, ele ira redimensionar
            var arquivosEntrada = Directory.EnumerateFiles(diretorioEntrada);

            if (arquivosEntrada.Count() == 0) imagesSequence = false;


            // ler o tamanho que ira redimensionar
            foreach (var arquivo in arquivosEntrada) {
                fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                fileInfo = new FileInfo(arquivo);


                using (Image imagemOriginal = Image.Load(fileStream)) {
                    foreach (var alturaAlvo in listaAltura) {
                        string extensao = fileInfo.Extension;
                        string nomeSemExtensao = Path.GetFileNameWithoutExtension(arquivo);
                        string novoNome = $"{nomeSemExtensao}_H{alturaAlvo}_{DateTime.Now:dd_MM_yy_HHmmss}{extensao}";

                        string caminho = Path.Combine(Environment.CurrentDirectory, diretorioRedimensionado, novoNome);

                        //Redimensiona & Copia os arquivos redimensionados para a pasta de redimensionados
                        Redimensionador(imagemOriginal.Clone(x => { }), alturaAlvo, caminho);
                    }
                }

                //Fecha o arquivo
                fileStream.Close();


                //move arquivo de entrada para a pasta de finalizados
                string caminhoFinalizado = Path.Combine(Environment.CurrentDirectory, diretorioFinalizado, fileInfo.Name);
                if(File.Exists(caminhoFinalizado)) File.Delete(caminhoFinalizado);

                fileInfo.MoveTo(caminhoFinalizado);
            }

            if(imagesSequence) Console.WriteLine($"All files resized!");
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