namespace Jsgaona
{
    public interface IHackeable
    {
        float TimeStopMotion { get; set; } // segundos que se detiene el agente
        bool ItsHacked { get; set; }        // estado de hack
        void Hack(float timeHack);
    }

}