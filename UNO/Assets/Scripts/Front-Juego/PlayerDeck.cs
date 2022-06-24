using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Cards
{
    public class PlayerDeck : MonoBehaviour
    {
        public GameObject cartaMedio;
        public GameObject carta1J1, carta2J1, carta3J1, carta4J1;
        public GameObject boton1J1, boton2J1, boton3J1, boton4J1;
        public GameObject textoNoTurno, textoNoCoincide;
        public GameObject colorRojo, colorVerde, colorAmarillo, colorAzul;
        public GameObject masCartas, masCartasIzq; 
        public GameObject botonUno;
        public GameObject temporizador;
        public GameObject CambiarColor; 
        public GameObject popUpFinPartida;
        public Text msgFinPartida;
        public Text msgJugada;

        public CharacterShopDatabase characterDB;

        //Cartas para representar el movimiento de otro jugador enviando carta al centro:
        public GameObject cartaJ2, cartaJ3, cartaJ4;        
        //Desk del resto de jugadores:
        public GameObject deckPlayerJ2, deckPlayerJ3, deckPlayerJ4;
        //Nombre de los jugadores:
        public Text nombreP1, nombreP2, nombreP3, nombreP4;
        public Image avatarJ1, avatarJ2, avatarJ3, avatarJ4;

        public List<int> indicesCartas = new List<int>();   //Nuestra baraja


        //Variables auxiliares
        private DisplayCard cartaM, carta, cartaContraria;
        private DisplayCard carta1, carta2, carta3, carta4;
        private GameObject Ccarta,Cboton;
        public string infoColor;
        private List<Vector2> posicionesBotonUno = new List<Vector2>(); //Tendra 5 posiciones aleatorias
        private int ID_BACK_CARD = 41;
        private int ganar = 0;
        private int esMiTurno = 0;
        private int primeraJugada = 0;
        private IEnumerator coroutine;
        private int posicion = 0;
        private int boton;
        private string sala;
        private int inicio = 0;
        private int turnosSinJugar = 0;
        public Text tiempoText;
        public string colorSeleccionado = "";
        private static float tiempo = 20;
        private int botonUnoPulsado = 0;
        private string anteriorJugador;
        private string siguienteJugador;
        private int numJugadores = 0;
        private int idJugado = 0;
    
    
        // Cargamos de la base de datos todas cartas
        void Start()
        {
            CambiarColor.SetActive(false);
            colorRojo.SetActive(false);
            colorVerde.SetActive(false);
            colorAmarillo.SetActive(false);
            colorAzul.SetActive(false);

            cartaMedio.SetActive(true);
            textoNoTurno.SetActive(false);
            textoNoCoincide.SetActive(false);
            botonUno.SetActive(false);
            temporizador.SetActive(false);
            popUpFinPartida.SetActive(false);
            

            //Añadimos las posiciones aleatorias del boton uno
            posicionesBotonUno.Add(new Vector2(300.0f,620.0f));
            posicionesBotonUno.Add(new Vector2(900.0f,500.0f));
            posicionesBotonUno.Add(new Vector2(150.0f,100.0f));
            posicionesBotonUno.Add(new Vector2(200.0f,200.0f));
            posicionesBotonUno.Add(new Vector2(1200.0f,200.0f));

            carta1 = carta1J1.GetComponent("DisplayCard") as DisplayCard;
            carta2 = carta2J1.GetComponent("DisplayCard") as DisplayCard;
            carta3 = carta3J1.GetComponent("DisplayCard") as DisplayCard;
            carta4 = carta4J1.GetComponent("DisplayCard") as DisplayCard;
            cartaM = cartaMedio.GetComponent("DisplayCard") as DisplayCard;

            //Miramos cual es el nombre de nuestra sala
            sala = GameController.sala;

            //Aqui tengo que mirar los datos de los jugadores y mi turno
            asignarPerfilJugadores();

            if(GameController.numJugadores == 3){
                deckPlayerJ4.SetActive(false);
                cartaJ4.SetActive(false);
            } else if(GameController.numJugadores == 2){
                deckPlayerJ3.SetActive(false);
                deckPlayerJ4.SetActive(false);
                cartaJ3.SetActive(false);
                cartaJ4.SetActive(false);
            }

            //Pedimos al servidor que nos envie el mazo inicial de cartas
            pedirCartas(7);

            inicio = 1;
            if(esMiTurno == 1) primeraJugada = 1;
        }

        // Update is called once per frame
        void Update()
        {
            //Mostramos nuestro mazo una vez recibido
            /*if((indicesCartas.Count > 0) && (inicio == 1)){
                carta1.laCarta(indicesCartas[0]);
                carta2.laCarta(indicesCartas[1]);
                carta3.laCarta(indicesCartas[2]);
                carta4.laCarta(indicesCartas[3]);

                inicio = 0;
            }*/

            if(indicesCartas.Count > 0) actualizarVista();

            if(indicesCartas.Count <= 4) apagaMasCartas();
            else enciendeMasCartas();
            
            if((esMiTurno == 1) && (tiempo > 0)){
                temporizador.SetActive(true);
                tiempo -= Time.deltaTime;
                tiempoText.text = "" + tiempo.ToString("f0");
            } else if((esMiTurno == 0) || (tiempo <= 0)){
                esMiTurno = 0;
                temporizador.SetActive(false);
                //Comprobamos si se ha quedado sin tiempo
                if(tiempo <= 0){
                    turnosSinJugar++;
                    ClientSend.SendCard(sala, ID_BACK_CARD, "TU TURNO"); //enviamos una carta sin valor
                    
                } else{
                    turnosSinJugar = 0;
                } 

                tiempo = 20;
                tiempoText.text = "" + tiempo.ToString("f0");
            }

            if(turnosSinJugar >= 3) ClientSend.BorrarDePartida(sala);

            if((indicesCartas.Count == 0) && (inicio == 0) && (ganar == 0)){
                msgFinPartida.text = "Eres el ganador de la partida, enhorabuena";
                hasGanado();
                ganar = 1;
            }
        }


        /************************************ Enviar cartas al centro ***************************************************/

        public void enviarCartaAlCentro(int i){ 
            boton = i;

            if(i == 1) comprobarTurnoYEnviar(carta1J1, boton1J1);
            else if(i == 2) comprobarTurnoYEnviar(carta2J1, boton2J1);
            else if(i == 3) comprobarTurnoYEnviar(carta3J1, boton3J1);
            else comprobarTurnoYEnviar(carta4J1, boton4J1);
        }
        

        /************************************** Jugada Contraria *******************************************************/
        //Envia jugada que ha pasado el server
        public void jugadaContraria(string jugador, int id, string info){
            
            boton1J1.SetActive(false);
            boton2J1.SetActive(false);
            boton3J1.SetActive(false);
            boton4J1.SetActive(false);

            idJugado = id;
            infoColor = info;

            //Hacemos el movimiento de mover la carta y la cambiamos en la del medio, si no recibimos carta parte de atras
            if(id != ID_BACK_CARD) hacerJugadaRival(jugador, info);
            else{
                boton1J1.SetActive(true);
                boton2J1.SetActive(true);
                boton3J1.SetActive(true);
                boton4J1.SetActive(true);
            }
            //Si hay cambio de turno, actualizamos turno
            comprobarTurnoTrasJugadaContraria(jugador, id);
            
            if(esMiTurno == 1){ // si es mi turno la posible carta especial me va afectar
                switch (id){
                case 13: // elige color
                    coroutine = alertaCambioColor(info);
                    StartCoroutine(coroutine);

                    //juegaNormal
                    break;

                case 27: // +4
                    pedirCartas(4);
                    coroutine = alertaCambioColor(info);
                    StartCoroutine(coroutine);

                    break;

                case 0: case 14: case 28: case 42: //saltar turno
                    if(info == ""){
                        ClientSend.SendCard(sala, ID_BACK_CARD, "TU TURNO"); //enviamos una carta sin valor
                        esMiTurno = 0;
                    }
                    break;
                case 2: case 16: case 30: case 44: //+2
                    pedirCartas(2);
                    break;
                }
            }
        }

        //Busca que jugador ha hecho la jugada para mover la carta
        private void hacerJugadaRival(string jugador, string info){
            if(nombreP2.text == jugador) gestionarCartaRival(jugador, cartaJ2, info);
            else if(nombreP3.text == jugador)  gestionarCartaRival(jugador, cartaJ3, info);
            else if(nombreP4.text == jugador)  gestionarCartaRival(jugador, cartaJ4, info);
        }

        private void gestionarCartaRival(string jugador, GameObject cartaObject, string info){
            cartaContraria = cartaObject.GetComponent("DisplayCard") as DisplayCard;

            cartaContraria.laCarta(idJugado);              //Le damos valor a la carta

            generarMsgJugada(jugador, idJugado, info);

            cartaObject.SetActive(true);                   //La hacemos visible

            cartaContraria.mover = 1; //Mover la carta al centro
            coroutine = EsperarACartaContraria(cartaObject);
            StartCoroutine(coroutine);
            cartaContraria.movimientoContrario = 0;
            
        }

        private void generarMsgJugada(string jugador, int id, string info){
            switch (id){
            case 13: // elige color
                msgJugada.text = "El jugador \"" + jugador + "\" ha lanzado un cambio de color (cambia a " + info + ")";
                break;
            case 27: // +4
                msgJugada.text = "El jugador \"" + jugador + "\" ha lanzado un +4 (y cambia color a " + info + ")";
                break;
            case 0: case 14: case 28: case 42: //saltar turno
                msgJugada.text = "El jugador \"" + jugador + "\" ha lanzado salto de turno de color " + CardDatabase.cardList[id].color;
                break;
            case 2: case 16: case 30: case 44: //+2
                msgJugada.text = "El jugador \"" + jugador + "\" ha lanzado un +2 de color " + CardDatabase.cardList[id].color;
                break;
            default:
                msgJugada.text = "El jugador \"" + jugador + "\" ha lanzado una carta normal de color " + CardDatabase.cardList[id].color;
                break;
            }
        }

        private void comprobarTurnoTrasJugadaContraria(string jugador, int id){
            string jugadorAux;
            
            if(numJugadores == 2){
                if((id == 1) || (id == 15) || (id == 29) || (id == 43)) esMiTurno = 0;
                else esMiTurno = 1;
            } else{
                if((id == 1) || (id == 15) || (id == 29) || (id == 43)){
                    //Controlamos el caso de cuando tiran un cambio de sentido, reajustamos turno, pero hay que seguir manteniendo estos cambios
                    if(anteriorJugador == jugador){
                        esMiTurno = 0;  //Pierdo turno

                        //Invertimos el orden de quien va antes y despues de nostros
                        jugadorAux = anteriorJugador;
                        anteriorJugador = siguienteJugador; 
                        siguienteJugador = jugadorAux;
                    } else if(siguienteJugador == jugador){
                        esMiTurno = 1;  //Gano turno

                        //Invertimos el orden de quien va antes y despues de nostros
                        jugadorAux = siguienteJugador;
                        siguienteJugador = anteriorJugador;
                        anteriorJugador = jugadorAux; 
                    }else{ //si llegamos aqui significa que quien ha jugado el cambio de sentido no es ni el jugador anterior ni el siguiente si no el que tenemos enfrente
                        esMiTurno = 0;  //Gano turno
                        jugadorAux = siguienteJugador;
                        siguienteJugador = anteriorJugador;
                        anteriorJugador = jugadorAux; 
                    }
                } else if(jugador == anteriorJugador){
                    esMiTurno = 1;
                } else{
                    esMiTurno = 0;
                }
            }
        }

        /**************************************** Jugador Eliminado ***************************************************/
        //Recibimos el nombre del jugador que se ha ido, hay que cambiar el tablero y turnos
        public void jugadorEliminado(string nombre){
            if(nombre == nombreP2.text){
                cartaJ2.SetActive(false);
                deckPlayerJ2.SetActive(false);
                nombreP2.text = " ";

                //Gestionar turnos
                generarTurnosJEliminado(nombre);
                //popUpFinPartida.SetActive(true);
                                  
            } else if(nombre == nombreP3.text){
                cartaJ3.SetActive(false);
                deckPlayerJ3.SetActive(false);
                nombreP3.text = " ";

                //Gestionar turnos
                generarTurnosJEliminado(nombre);               
            } else if(nombre == nombreP4.text){
                cartaJ4.SetActive(false);
                deckPlayerJ4.SetActive(false);
                nombreP4.text = " ";              
            } else{
            }
        }

        private void generarTurnosJEliminado(string nombre){
            if(numJugadores == 2 && ganar == 0){
                msgFinPartida.text = "Todos han abandonado, eres el ganador de la partida";
                hasGanado();
                ganar = 1;
            } else if(numJugadores == 3){
                //Pasamos a tener 2 jugadores, por lo que no hace falta hacer nada con los turnos (creo)
            } else if(numJugadores == 4){
                if(nombre == siguienteJugador){
                    //Da igual cual de los dos se va, nuestro sig jugador sera el de arriba
                    siguienteJugador = nombreP4.text;
                } else if(nombre == anteriorJugador){
                    //Da igual cual de los dos se va, nuestro anterior jugador sera el de arriba
                    anteriorJugador = nombreP4.text;
                    esMiTurno = 1;
                }
            }

            if(nombre == anteriorJugador) esMiTurno = 1;

            numJugadores--;
        }

        /**************************************** Anteriores Cartas ***************************************************/
        public void anterioresCartas(){
            carta1 = carta1J1.GetComponent("DisplayCard") as DisplayCard;
            carta2 = carta2J1.GetComponent("DisplayCard") as DisplayCard;
            carta3 = carta3J1.GetComponent("DisplayCard") as DisplayCard;
            carta4 = carta4J1.GetComponent("DisplayCard") as DisplayCard;
            if(posicion == 0){ //estas al principio de la baraja
                int conteo = indicesCartas.Count % 4;
                if(conteo == 0){ // mostramos todas las cartas
                    posicion = indicesCartas.Count - 4;
                    carta1.laCarta(indicesCartas[posicion]);
                    carta2.laCarta(indicesCartas[posicion + 1]);
                    carta3.laCarta(indicesCartas[posicion + 2]);
                    carta4.laCarta(indicesCartas[posicion + 3]);
                }else if(conteo == 3){ // mostramos 3 cartas
                    posicion = indicesCartas.Count - 3;
                    carta1.laCarta(indicesCartas[posicion]);
                    carta2.laCarta(indicesCartas[posicion + 1]);
                    carta3.laCarta(indicesCartas[posicion + 2]);
                    carta4.laCarta(ID_BACK_CARD);
                }else if(conteo == 2){ // mostramos 2 cartas
                    posicion = indicesCartas.Count - 2;
                    carta1.laCarta(indicesCartas[posicion]);
                    carta2.laCarta(indicesCartas[posicion + 1]);
                    carta3.laCarta(ID_BACK_CARD);
                    carta4.laCarta(ID_BACK_CARD);
                }else{
                    posicion = indicesCartas.Count - 1;
                    carta1.laCarta(indicesCartas[posicion]);
                    carta2.laCarta(ID_BACK_CARD);
                    carta3.laCarta(ID_BACK_CARD);
                    carta4.laCarta(ID_BACK_CARD);
                }
            }else{ //simplemente estas a la mitad, asi que siempre que vayas para atras te encontraras con 4 cartas
                posicion = posicion - 4;
                carta1.laCarta(indicesCartas[posicion]);
                carta2.laCarta(indicesCartas[posicion + 1]);
                carta3.laCarta(indicesCartas[posicion + 2]);
                carta4.laCarta(indicesCartas[posicion + 3]);
            }
        }

        /**************************************** Sig cartas **********************************************************/
        //Funcion encargada de al darle a "mas cartas", pasara a tus siguientes 4 cartas.
        private void siguientesCartas(){
            // Se declaran las cartas del jugador como componentes para poder manejarlas 
            carta1 = carta1J1.GetComponent("DisplayCard") as DisplayCard;
            carta2 = carta2J1.GetComponent("DisplayCard") as DisplayCard;
            carta3 = carta3J1.GetComponent("DisplayCard") as DisplayCard;
            carta4 = carta4J1.GetComponent("DisplayCard") as DisplayCard;
            
            int NCartas = indicesCartas.Count;
            int paginaSiguiente = posicion + 4; // "PaginaSiguiente" es la primera posicion de la pagina siguiente 
            
            // Si hay mas cartas que las caben en la pagina siguiente se siguen mostrando
            if(NCartas > paginaSiguiente){
                if (NCartas - 1 >= paginaSiguiente){
                    carta1.laCarta(indicesCartas[paginaSiguiente]);
                    if (NCartas - 1 >= paginaSiguiente + 1){
                        carta2.laCarta(indicesCartas[paginaSiguiente + 1]);
                        if (NCartas - 1 >= paginaSiguiente + 2){
                            carta3.laCarta(indicesCartas[paginaSiguiente + 2]);
                            if (NCartas - 1>= paginaSiguiente + 3){
                                carta4.laCarta(indicesCartas[paginaSiguiente + 3]);
                            } else {
                                carta4.laCarta(ID_BACK_CARD);
                            }
                        } else{
                            carta3.laCarta(ID_BACK_CARD);
                            carta4.laCarta(ID_BACK_CARD);
                        }
                    } else{
                        carta2.laCarta(ID_BACK_CARD);
                        carta3.laCarta(ID_BACK_CARD);
                        carta4.laCarta(ID_BACK_CARD);
                    }
                }
                posicion = paginaSiguiente; //se actualiza la ultima posicion ya que nos hemos metido en la condicion
            }  
            // La siguiente pagina sera la primera porque hemos acabado de ver todas las cartas de la mano                    
            else if(paginaSiguiente > NCartas - 1){ 
                // Actualizamos posicion porque hemos vuelto a mostrar las primeras cartas
                posicion = 0;
                if (NCartas - 1 >= 1){
                    carta1.laCarta(indicesCartas[posicion]);
                    if (NCartas - 1 >= 2){
                        carta2.laCarta(indicesCartas[posicion + 1]);
                        if (NCartas - 1 >= 3){
                            carta3.laCarta(indicesCartas[posicion + 2]);
                            if (NCartas - 1 >= 4){
                                carta4.laCarta(indicesCartas[posicion + 3]);
                            } else {
                                carta4.laCarta(ID_BACK_CARD);
                            }
                        } else{
                            carta3.laCarta(ID_BACK_CARD);
                            carta4.laCarta(ID_BACK_CARD);
                        }
                    } else{
                        carta2.laCarta(ID_BACK_CARD);
                        carta3.laCarta(ID_BACK_CARD);
                        carta4.laCarta(ID_BACK_CARD);
                    }
                }
            }
        }


        /**************************************** Coger carta ********************************************************/

        private void cogerCarta(){
            if(esMiTurno == 1){
                //LLamamos al server para que nos de una carta, pero solo 1
                pedirCartas(1);
            } else{
                textoNoTurno.SetActive(true);
                coroutine = MsgError(textoNoTurno);
                StartCoroutine(coroutine);
            }
        }


        /**************************************** Cantar carta ********************************************************/

        private void cantarUno(){
            botonUnoPulsado = 1;
        }


        /************************************ Funciones auxiliares ***************************************************/
        //actualizar vista
        public void actualizarVista(){
            inicio = 0;
            carta1 = carta1J1.GetComponent("DisplayCard") as DisplayCard;
            carta2 = carta2J1.GetComponent("DisplayCard") as DisplayCard;
            carta3 = carta3J1.GetComponent("DisplayCard") as DisplayCard;
            carta4 = carta4J1.GetComponent("DisplayCard") as DisplayCard;

            int posFin = posicion + 3;
            if(indicesCartas.Count == 1){
                carta1.laCarta(indicesCartas[posicion]);
                carta2.laCarta(41);
                carta3.laCarta(41);
                carta4.laCarta(41);
            }
            else if(posFin <= indicesCartas.Count - 1){//si hay cuatro cartas para mostrar las mostramos todas
                carta1.laCarta(indicesCartas[posicion]);
                carta2.laCarta(indicesCartas[posicion + 1]);
                carta3.laCarta(indicesCartas[posicion + 2]);
                carta4.laCarta(indicesCartas[posicion + 3]);
            } else{
                if(posFin - 1 <= indicesCartas.Count - 1){ //si hay 3 cartas al final para mostrar
                    carta1.laCarta(indicesCartas[posicion]);
                    carta2.laCarta(indicesCartas[posicion + 1]);
                    carta3.laCarta(indicesCartas[posicion + 2]);
                    carta4.laCarta(41);
                } else{
                    if(posFin - 2 <= indicesCartas.Count - 1){ //si hay 2 cartas para mostrar
                        carta1.laCarta(indicesCartas[posicion]);
                        carta2.laCarta(indicesCartas[posicion + 1]);
                        carta3.laCarta(41);
                        carta4.laCarta(41);
                    } 
                }
            }
        }

        //Pedimos cartas al server
        private void pedirCartas(int numCartas){
            ClientSend.RobarCartas(sala, numCartas);
        }

        //Funcion encargada de una vez enviada la carta al centro, desplaza las cartas de tu mazo a la izq.
        private void actualizarBaraja(){
            indicesCartas.Remove(carta.id);
            if(indicesCartas.Count == 0){ // es la ultima carta
                carta1.laCarta(41);
                carta2.laCarta(41);
                carta3.laCarta(41);
                carta4.laCarta(41);
            } else{
                if(indicesCartas.Count - 1 < posicion){
                    posicion = posicion - 4;
                }
                bool faltan = true;
                for(int i = posicion;(i < posicion + 4) && (indicesCartas.Count > i); i ++){
                    if(i == posicion + 3){ //significa que podemos mostrar las 4 cartas
                        faltan = false;
                        carta4.laCarta(indicesCartas[i]);
                    }else if(i == posicion){
                        carta1.laCarta(indicesCartas[i]);
                    }else if(i == posicion + 1){
                        carta2.laCarta(indicesCartas[i]);
                    }else if(i == posicion + 2){
                        carta3.laCarta(indicesCartas[i]);
                    }
                }

                if(faltan == true){ //las cartas restantes se configuran con la imagen predeterminada
                    int cont = indicesCartas.Count%4;
                    if (cont  == 3){
                        carta4.laCarta(41);
                    }else if(cont == 2){
                        carta3.laCarta(41);
                        carta4.laCarta(41);
                    }else if(cont == 1){
                        carta2.laCarta(41);
                        carta3.laCarta(41);
                        carta4.laCarta(41);
                    }
                }   
            }
        }

        /************************************** Selecciona color *******************************************************/
        public void escogeColor(int i){
            CambiarColor.SetActive(false);
            switch (i){
                case 1: // Verde
                    ClientSend.SendCard(sala, carta.id, "Verde"); //lo envia bien?
                    break;

                case 2: // Rojo
                    ClientSend.SendCard(sala, carta.id, "Rojo"); //lo envia bien?
                    break;

                case 3: //Azul
                    ClientSend.SendCard(sala, carta.id, "Azul"); //lo envia bien?
                    break;
                
                case 4: //Amarillo
                    ClientSend.SendCard(sala, carta.id, "Amarillo"); //lo envia bien?
                    break;
                default:
                    //juegaComoSea
                    break;
            }            
            //Si son el mismo numero, si son del mismo color o si no se ha introducido ninguna carta aun
            carta.mover = 1; //Mover la carta al centro
            coroutine = EsperarACarta(Ccarta, Cboton);
            StartCoroutine(coroutine);
            carta.apagate = 0; //TO DO: esto tiene sentido aqui¿?

            esMiTurno = 0;
            
        }


        /************************************** Comprobar turno y jugar *******************************************************/       
        private void comprobarTurnoYEnviar(GameObject cartaObject, GameObject botonObject){
            carta = cartaObject.GetComponent("DisplayCard") as DisplayCard;

            if(esMiTurno == 1){
                if(((carta.id == 27) || (carta.id == 13)) || (((carta.id == 27) || (carta.id == 13)) && (primeraJugada == 1))){
                    //Vamos a jugar una carta que conllevara un cambio de color
                    idJugado = carta.id;
                    Ccarta = cartaObject;
                    Cboton = botonObject;
                    CambiarColor.SetActive(true);
                    primeraJugada = 0;
                }else if((cartaM.id == 27) || (cartaM.id == 13)){ 
                    //Alguien ha jugado una carta que lleva un cambio de color
                    if(infoColor == carta.color){
                        gestionarTurnoTrasCambio(cartaObject, botonObject);
                        infoColor = "";
                    } else{
                        //Si no se puede poner
                        textoNoCoincide.SetActive(true);
                        coroutine = MsgError(textoNoCoincide);
                        StartCoroutine(coroutine);
                    }
                } else if ((cartaM.valor == carta.valor) || (cartaM.color == carta.color) || (primeraJugada == 1) || (cartaM.id == ID_BACK_CARD)){
                    //Si viene una carta normal
                    gestionarTurnoTrasCambio(cartaObject, botonObject);
                    primeraJugada = 0;
                } else{
                    //Si no se puede poner
                    textoNoCoincide.SetActive(true);
                    coroutine = MsgError(textoNoCoincide);
                    StartCoroutine(coroutine);
                }
            } else{
                //Si no es mi turno
                textoNoTurno.SetActive(true);
                coroutine = MsgError(textoNoTurno);
                StartCoroutine(coroutine);
            }
        }

        //Tras usar un cambio de turno propio, ajustamos nuestro turno
        private void gestionarTurnoTrasCambio(GameObject cartaObject, GameObject botonObject){
            if((carta.id == 1) || (carta.id == 15) || (carta.id == 29) || (carta.id == 43)){
                if(numJugadores == 2){
                    esMiTurno = 1;
                } else{                            
                    string jugadorAux = siguienteJugador;
                    siguienteJugador = anteriorJugador;
                    anteriorJugador = jugadorAux;                            

                    esMiTurno = 0;
                }
            } else{
                esMiTurno = 0;
            }  

            idJugado = carta.id;
            ClientSend.SendCard(sala, carta.id, "");

            //Si son el mismo numero, si son del mismo color o si no se ha introducido ninguna carta aun
            carta.mover = 1; //Mover la carta al centro
            coroutine = EsperarACarta(cartaObject, botonObject);
            StartCoroutine(coroutine);
            carta.apagate = 0;
        }

        /*********************************************************************************************************************/


        private void mostrarBotonUno(){
            //Lo colocamos en una posicion aleatoria
            System.Random rand = new System.Random();
            int randNum = rand.Next(1,5);

            botonUno.transform.position = posicionesBotonUno[randNum];
            
            //Lo hacemos visible
            botonUno.SetActive(true);
        
            //Esperamos 2 segundo para ver si lo pulsa
            StartCoroutine(esperarBotonUno());
        }

        private void apagaMasCartas(){
            masCartas.SetActive(false);
            masCartasIzq.SetActive(false);
        }

        private void enciendeMasCartas(){
            masCartas.SetActive(true);
            masCartasIzq.SetActive(true);
        }

        private void asignarPerfilJugadores(){
            esMiTurno = GameController.miTurno;
            nombreP1.text = GameController.miNombreJugador;
            string avatarName = GameController.miAvatarAux;
            //avatarName = "2";
            int a = Int32.Parse(avatarName);
            Character character = characterDB.GetCharacter(a);
            avatarJ1.sprite = character.image;
            //TODO: necesito mi foto perfil

            if(GameController.numJugadores == 2){
                nombreP2.text = GameController.jugadores[0].nombreJugador;
                a = Int32.Parse(GameController.jugadores[0].avatar);
                character = characterDB.GetCharacter(a);
                avatarJ2.sprite = character.image;
            } else if(GameController.numJugadores == 3){
                bool opcion1, opcion2;
                
                for(int i = 0; i < 2; i++){
                    //El jugador que va a la izquierda:
                    opcion1 =   (GameController.miTurno - 1 == GameController.jugadores[i].turno) || 
                                (GameController.miTurno + 2 == GameController.jugadores[i].turno);
                    //El jugador que va a la derecha:
                    opcion2 =   (GameController.miTurno + 1 == GameController.jugadores[i].turno) || 
                                (GameController.miTurno - 2 == GameController.jugadores[i].turno);

                    if(opcion1){
                        nombreP2.text = GameController.jugadores[i].nombreJugador;
                        a = Int32.Parse(GameController.jugadores[i].avatar);
                        character = characterDB.GetCharacter(a);
                        avatarJ2.sprite = character.image;
                    } else if(opcion2){
                        nombreP3.text = GameController.jugadores[i].nombreJugador;
                        a = Int32.Parse(GameController.jugadores[i].avatar);
                        character = characterDB.GetCharacter(a);
                        avatarJ3.sprite = character.image;
                    }
                }
            } else if(GameController.numJugadores == 4){
                bool opcion1, opcion2, opcion3;
                
                for(int i = 0; i < 3; i++){
                    //El jugador que va a la izquierda:
                    opcion1 =   (GameController.miTurno - 1 == GameController.jugadores[i].turno) || 
                                (GameController.miTurno + 3 == GameController.jugadores[i].turno);
                    //El jugador que va a la derecha:
                    opcion2 =   (GameController.miTurno + 1 == GameController.jugadores[i].turno) || 
                                (GameController.miTurno - 3 == GameController.jugadores[i].turno);
                    //El jugador que va arriba:
                    opcion3 =   (GameController.miTurno + 2 == GameController.jugadores[i].turno) || 
                                (GameController.miTurno - 2 == GameController.jugadores[i].turno);

                    if(opcion1){
                        nombreP2.text = GameController.jugadores[i].nombreJugador;
                        a = Int32.Parse(GameController.jugadores[i].avatar);
                        character = characterDB.GetCharacter(a);
                        avatarJ2.sprite = character.image;
                    } else if(opcion2){
                        nombreP3.text = GameController.jugadores[i].nombreJugador;
                        a = Int32.Parse(GameController.jugadores[i].avatar);
                        character = characterDB.GetCharacter(a);
                        avatarJ3.sprite = character.image;
                    } else if(opcion3){ 
                        nombreP4.text = GameController.jugadores[i].nombreJugador;
                        a = Int32.Parse(GameController.jugadores[i].avatar);
                        character = characterDB.GetCharacter(a);
                        avatarJ4.sprite = character.image;
                    }
                }
            }

            //Guardamos el jugador de la izq, es quien nos preccede en el turno
            anteriorJugador = nombreP2.text;
            siguienteJugador = nombreP3.text;
            numJugadores = GameController.numJugadores;
        }


        /******************************** Funcion tras ganar partida **********************************************/
        private void hasGanado(){
            ClientSend.GanarPartida(sala, nombreP1.text);
            popUpFinPartida.SetActive(true);

            //Desbloqueo de logros
            controladorLogro cL = GameObject.Find("controladorLogros").GetComponent<controladorLogro>();
			cL.gana1Partida(); 
            cL.gana10Partidas();
            cL.gana5Partidas();

        }

        /********************************** Funcion menu principal ************************************************/
        public void volverMenuPrincipal(){
            SceneManager.LoadScene("MenuPrincipal");
        }

        /********************************** Funcion salir al menu *************************************************/
        public void salirDelJuego(){
            ClientSend.BorrarDePartida(sala);
            volverMenuPrincipal();
        }

        /********************************** Funcion ha ganado otro ************************************************/
        public void hayGanador(string ganador){
            msgFinPartida.text = "El ganador es " + ganador + ", has perdido";
            popUpFinPartida.SetActive(true);
        }


        /********************************** Funciones con retardos ************************************************/

        IEnumerator EsperarACarta(GameObject cartaObject, GameObject botonObject){
            //Para que no genere tanto delay en las cartas del medio
            carta.speed = 500;
            if ((boton == 1) || (boton == 0)) yield return new WaitForSeconds(1.5f);
            else  yield return new WaitForSeconds(1);

            //Que la carta del medio se quede con sus valores
            cartaM.laCarta(idJugado);

            //Si la carta del medio estaba quitada, se activa
            if (!(cartaMedio.activeSelf)) cartaMedio.SetActive(true);
            
            //Le decimos que no se mueva mas y que desaparezca
            carta.apagate = 1;
            carta.mover = 0;

            //Actualizamos la baraja una vez que la carta del medio a obtenido el valor de la carta usada
            actualizarBaraja();

            //Si solo tenemos una carta activamos variable de botonUno
            if(indicesCartas.Count == 1) mostrarBotonUno();
        }

        IEnumerator MsgError(GameObject texto){
            yield return new WaitForSeconds(1);
            texto.SetActive(false);
        }

        //Le damos un tiempo al jugador uan vez que aparece el boton para que pueda pulsar el boton de UNO
        IEnumerator esperarBotonUno(){
            yield return new WaitForSeconds(2);

            if(botonUnoPulsado != 1){
                //Pedimos carta si no le ha dado al boton
                pedirCartas(1); 
                actualizarBaraja();
            }

            botonUnoPulsado = 0;
            botonUno.SetActive(false);
        }


        IEnumerator alertaCambioColor(string color){ 
            if(color == "Rojo"){
                colorRojo.SetActive(true);
                yield return new WaitForSeconds(2);
                colorRojo.SetActive(false);
            }else if(color == "Verde"){
                colorVerde.SetActive(true);
                yield return new WaitForSeconds(2);
                colorVerde.SetActive(false);
            }else if(color == "Amarillo"){
                colorAmarillo.SetActive(true);
                yield return new WaitForSeconds(2);
                colorAmarillo.SetActive(false);
            }else if(color == "Azul"){
                colorAzul.SetActive(true);
                yield return new WaitForSeconds(2);
                colorAzul.SetActive(false);
            }
        }

        IEnumerator EsperarACartaContraria(GameObject cartaObject){
            cartaContraria.speed = 500;
            yield return new WaitForSeconds(1.5f);

            //Que la carta del medio se quede con sus valores
            cartaM.laCarta(cartaContraria.id);

            //Si la carta del medio estaba quitada, se activa
            if (!(cartaMedio.activeSelf)) cartaMedio.SetActive(true);

            cartaContraria.movimientoContrario = 1;

            boton1J1.SetActive(true);
            boton2J1.SetActive(true);
            boton3J1.SetActive(true);
            boton4J1.SetActive(true);
        }  
    }
}

