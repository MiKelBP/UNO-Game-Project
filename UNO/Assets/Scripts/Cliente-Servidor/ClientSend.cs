using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write("Pedro Picapiedra");

            SendTCPData(_packet);
        }
    }

    public static void SendCard(string sala, int id, string infoExtra)
    {
        using (Packet _packet = new Packet((int)ClientPackets.cartaCliente))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(sala);
            _packet.Write(id);
            _packet.Write(infoExtra);

            SendTCPData(_packet);
        }
    }

    public static void SendRegister(string user, string mail, int hash)
    {
        using (Packet _packet = new Packet((int)ClientPackets.registro))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(user);
            _packet.Write(mail);
            _packet.Write(hash);

            SendTCPData(_packet);
        }
    }

    public static void SendLogin(string user, int hash)
    {
        using (Packet _packet = new Packet((int)ClientPackets.login))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(user);
            _packet.Write(hash);

            SendTCPData(_packet);
        }
    }

    public static void EntraSala(string sala, bool esCreador, string name, string avatar)
    {
        using (Packet _packet = new Packet((int)ClientPackets.entraSala))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(sala);
            _packet.Write(name);
            _packet.Write(avatar);
            _packet.Write(esCreador);
            _packet.Write(false); //Version ejecutable

            SendTCPData(_packet);
        }
    }

    public static void RobarCartas(string sala, int numCartas)
    {
        using (Packet _packet = new Packet((int)ClientPackets.robarCartas))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(sala);
            _packet.Write(numCartas);

            SendTCPData(_packet);
        }
    }

    public static void EstoyListo(string sala)
    {
        using (Packet _packet = new Packet((int)ClientPackets.estoyListo))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(sala);

            SendTCPData(_packet);
        }
    }

    public static void PedirSalas()
    {
        using (Packet _packet = new Packet((int)ClientPackets.pedirSalas))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void BorrarDeSala(string sala)
    {
        using (Packet _packet = new Packet((int)ClientPackets.borrarDeSala))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(sala);

            SendTCPData(_packet);
        }
    }

    public static void BorrarDePartida(string sala)
    {
        using (Packet _packet = new Packet((int)ClientPackets.borrarDePartida))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(sala);

            SendTCPData(_packet);
        }
    }

    public static void GanarPartida(string sala, string nombre)
    {
        using (Packet _packet = new Packet((int)ClientPackets.heGanado))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(sala);
            _packet.Write(nombre);

            SendTCPData(_packet);
        }
    }

    #endregion
}
