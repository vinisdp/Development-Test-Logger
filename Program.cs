internal class Program
{
    private static async Task Main(string[] args)
    {
        string traceKey = Guid.NewGuid().ToString(); // Gerando um traceKey único
        string source = "LoggerApp"; // Nome do serviço ou aplicação
        string httpAddress = "http://localhost:5000"; // Endereço HTTP do serviço

        Logger logger = new Logger(source, httpAddress, logFilePath: "app.log", minLevel: LogLevel.Info);

        await logger.Info("Aplicação iniciada.", traceKey);
        await logger.Debug("Este log não será salvo pois o nível mínimo é Info.", traceKey);
        await logger.Warning("Alerta: possível sobrecarga no sistema.", traceKey);
        await logger.Error("Erro crítico detectado!", traceKey);

        try
        {
            int x = 0;
            int y = 10 / x;
        }
        catch (Exception ex)
        {
            await logger.Error(ex, traceKey);
        }
    }
}
