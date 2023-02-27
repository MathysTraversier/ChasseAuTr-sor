using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ChasseAuTresor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow; //Pour changer la couleur du texte dans la console.
            Console.WriteLine("█▀▀ █░█ ▄▀█ █▀ █▀ █▀▀   ▄▀█ █░█   ▀█▀ █▀█ █▀▀ █▀ █▀█ █▀█");
            Console.WriteLine("█▄▄ █▀█ █▀█ ▄█ ▄█ ██▄   █▀█ █▄█   ░█░ █▀▄ ██▄ ▄█ █▄█ █▀▄");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            AfficherPhrase("Bienvenue dans le jeu de la Chasse Au Trésor ! Ici, votre but est d'amasser tous les trésors (T) de l'île en vous aidant des indices donnés par le maître du jeu. Sur votre chemin, prenez garde à ne pas vous faire exploser par les mines amorcées (M) pour les convoiteurs des trésors ! Bonne chance à vous et que la force soit avec vous...", true);
            Console.WriteLine();

            bool recommencerPartie; //On initie un booléen qui selon sa valeur à la fin du programme relancera une partie.

            do //La fin de cette boucle dépendera du booléen recommencerPartie.
            {
                bool endGame = false; //On initie un nouveau booléen qui selon sa valeur mettra fin à une partie.

                int score = 0;
                int[] tailleGrille = InitialiserTailleGrille(); //On initie un tableau qui récupérera les tailles de la grille entrées par l'utilisateur (nombre de lignes en 0 et nombre de colonnes en 1).
                int[,] grilleJeu = InitialiserGrilleJeu(tailleGrille[0], tailleGrille[1]); //On initie un tableau d'entiers qui répertorieriera l'emplacement des mines et des trésors (2, 1 et 0 correspondant respectivement à un trésor, une mine et rien).
                string[,] grilleJoueur = InitialiserGrilleJoueur(tailleGrille[0], tailleGrille[1]); //On initie le plateau du joueur (seul tableau que le joueur verra réellement).
                int nbMines = CreerNombreMines(grilleJeu); //On initie le nombre de mines.
                int nbTresors = CreerNombreTresors(grilleJeu, nbMines); //On initie le nombre de trésors.

                AfficherGrilleJoueur(grilleJoueur);

                int[] jeuDuJoueur = JouerCase(tailleGrille[0], tailleGrille[1], grilleJoueur); //On demande à l'utilisateur ce qu'il veut jouer.
                score++; //À chaque fois que le joueur joue, on augmente le score.
                PlacerMinesEtTresors(grilleJeu, jeuDuJoueur, nbTresors, nbMines);
                Jouer(jeuDuJoueur[0], jeuDuJoueur[1], grilleJeu, grilleJoueur);
                AfficherGrilleJoueur(grilleJoueur);

                while (!endGame) //On crée une boucle qui continuera tant que le joueur n'a pas perdu ou gagné : le joueur joue, son score augmente à chaque fois, on affiche la grille et on vérifie si la partie est terminée.
                {
                    jeuDuJoueur = JouerCase(tailleGrille[0], tailleGrille[1], grilleJoueur);
                    score++;
                    Jouer(jeuDuJoueur[0], jeuDuJoueur[1], grilleJeu, grilleJoueur);
                    AfficherGrilleJoueur(grilleJoueur);
                    endGame = IsGameOver(jeuDuJoueur[0], jeuDuJoueur[1], grilleJeu, grilleJoueur, nbTresors, nbMines);
                }

                AfficherResultat(jeuDuJoueur[0], jeuDuJoueur[1], grilleJoueur, grilleJeu, score);
                recommencerPartie = CommencerNouvellePartie(); //On met à jour le booléen pour savoir si le joueur veut recommencer une partie.
                Console.WriteLine();
            }
            while (recommencerPartie);

            Console.ReadLine();
        }

        static int[] InitialiserTailleGrille() //Cette fonction est faite pour initialiser la taille de la grille de jeu.
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("       CHOIX DE LA DIMENSION DE LA GRILLE DE JEU");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            bool conversion;

            AfficherPhrase("Saisissez le nombre de lignes de la grille de jeu (supérieur à 1) : ",false);
            int nbLignesGrille;
            conversion = int.TryParse(Console.ReadLine(), out nbLignesGrille); //On demande à l'utilisateur le nombre de lignes.
            while (nbLignesGrille <= 1 || !conversion) //Tant que le nombre de lignes entré par l'utilisateur n'est pas au moins 2 ou qu'il apparaît des caractères dans la saisie, il redemande à l'utilisateur d'entrer un nombre correct de lignes.
            {
                AfficherPhrase("Saisie incorrecte. Le nombre de lignes de la grille de jeu doit être un entier strictement supérieur à 1.",true);
                AfficherPhrase("Saisissez le nombre de lignes de la grille de jeu (supérieur à 1) : ", false);
                conversion = int.TryParse(Console.ReadLine(), out nbLignesGrille);
            }

            AfficherPhrase("Saisissez le nombre de colonnes de la grille de jeu (supérieur à 1) : ",false);
            int nbColonnesGrille;
            conversion = int.TryParse(Console.ReadLine(), out nbColonnesGrille); //On demande à l'utilisateur le nombre de colonnes.
            while (nbColonnesGrille <= 1 || !conversion) //De même que pour la boucle demandant le nombre de lignes.
            {
                AfficherPhrase("Saisie incorrecte. Le nombre de colonnes de la grille de jeu doit être un entier strictement supérieur à 1.",true);
                AfficherPhrase("Saisissez le nombre de colonnes de la grille de jeu (supérieur à 1) : ", false);
                conversion = int.TryParse(Console.ReadLine(), out nbColonnesGrille);
            }

            int[] tailleGrille = new int[] { nbLignesGrille, nbColonnesGrille }; //On entre les tailles de la grille de jeu dans un tableau que l'on retournera.

            return tailleGrille;

        }

        static int[,] InitialiserGrilleJeu(int nbLignesGrille, int nbColonnesGrille) //Cette fonction initialise la grille de jeu avec seulement des 0 puisque l'on ne place pas encore les mines et trésors.
        {
            int[,] grille = new int[nbLignesGrille, nbColonnesGrille];
            for (int row = 0; row < grille.GetLength(0); row++)
                for (int column = 0; column < grille.GetLength(1); column++)
                    grille[row, column] = 0;
            return grille;
        }

        static string[,] InitialiserGrilleJoueur(int nbLignesGrille, int nbColonnesGrille) //Cette fonction initialise la grille du joueur avec seulement des étoiles signifiant des cases non découvertes.
        {
            string [,] grille = new string[nbLignesGrille, nbColonnesGrille];
            for (int row = 0; row < grille.GetLength(0); row++)
                for (int column = 0; column < grille.GetLength(1); column++)
                    grille[row, column] = "* "; //On choisit par défaut de mettre la taille d'une variable string à 2 dans le tableau pour éviter la déformation du tableau lorsqu'une case possède un décompte à deux chiffres.
            return grille;
        }

        static int CreerNombreMines(int[,] grille) //Cette fonction sert à initialiser le nombre de mines.
        {
            Random mines = new Random();
            int nbMines = mines.Next(grille.GetLength(0) / 2, grille.Length / 2 + 1); //Le nombre aléatoire de mines est créé comme indiqué dans la consigne.
            return nbMines;
        }

        static int CreerNombreTresors(int[,] grille, int nbMines) //Cette fonction sert à initialiser le nombre de trésors.
        {
            Random tresors = new Random();
            int nbTresors;
            //Deux cas se présentent :
            if (grille.Length == 2 && nbMines == 2) //Si la taille de la grille est 2:2, Il se peut que le nombre de mines soit de 2. Par conséquent, il ne peut y avoir 3 trésors. On crée donc un chiffre aléatoire entre 1 et 2.
            {
                nbTresors = tresors.Next(1, 3); //Dans cette fonction, 3 est exclu.
            }
            else //Sinon on crée un chiffre aléatoire entre 1 et 3 comme demandé dans les consignes.
            {
                nbTresors = tresors.Next(1, 4);
            }
            return nbTresors;
        }

        static void PlacerMinesEtTresors(int [,] grille, int [] jeuDuJoueur, int nbTresors, int nbMines) //Cette fonction sert à placer les mines et trésors dans la grille.
        {
            for (int i = 0; i < nbMines; i++) //On crée une boucle for afin de placer chaque mine. Il y aura donc autant de tour de boucle que de nombre de mines.
            {
                int ligneDeLaMine, colonneDeLaMine;
                do
                {
                    Random ligne = new Random();
                    ligneDeLaMine = ligne.Next(0, grille.GetLength(0)); //On crée aléatoirement un nombre représentant une ligne de la grille où sera placée la mine.

                    Random colonne = new Random();
                    colonneDeLaMine = colonne.Next(0, grille.GetLength(1)); //On crée aléatoirement un nombre représentant une colonne de la grille où sera placée la mine.
                }
                while ((ligneDeLaMine == jeuDuJoueur[0] && colonneDeLaMine == jeuDuJoueur[1]) || grille[ligneDeLaMine,colonneDeLaMine] == 1); //On ne valide pas la position de la mine tant qu'elle se trouve sur la case jouée par le joueur ou qu'elle se trouve sur une mine déjà placée. On recrée par conséquent des positions aléatoires.


                grille[ligneDeLaMine, colonneDeLaMine] = 1; //On assigne la valeur 1 à la case de cette nouvelle mine (1 représentant une mine).
            }

            for (int i = 0; i < nbTresors; i++) //On refait la même chose avec les trésors mais avec la valeur 2 (2 représentant un trésor).
            {
                int ligneDuTresor, colonneDuTresor;
                do
                {
                    Random ligne = new Random();
                    ligneDuTresor = ligne.Next(0, grille.GetLength(0));

                    Random colonne = new Random();
                    colonneDuTresor = colonne.Next(0, grille.GetLength(1));
                }
                while ((ligneDuTresor == jeuDuJoueur[0] && colonneDuTresor == jeuDuJoueur[1]) || grille[ligneDuTresor,colonneDuTresor] == 1 || grille[ligneDuTresor,colonneDuTresor] == 2); //Attention, ici, il faut penser à l'éventualité qu'un trésor soit déjà placé à la position générée.

                grille[ligneDuTresor, colonneDuTresor] = 2;
            }
        }

        static void AfficherGrilleJoueur(string[,] grille) //Cette fonction sert à afficher le plateau du joueur.
        {
            Console.WriteLine();
            Console.Write("   ");
            for (int i = 1; i <= grille.GetLength(1); i++) //Cette boucle sert à afficher les numéros des colonnes.
            {
                if (i < 10)
                    Console.Write(" " + i + " ");
                else
                    Console.Write(" "+i);
            }
            Console.WriteLine();
            Console.Write("   ");
            for (int i = 0; i < 3*grille.GetLength(1)+1; i++) //Cette boucle for va servir à afficher la bordure supérieure du plateau. 3 représentant la place que prend une case et le +1 sert à fermer le tableau par la partie supérieure droite.
            {
                Console.Write("-");
            }
            Console.WriteLine();
            for (int i = 0; i < grille.GetLength(0); i++) //Cette boucle va permettre l'affichage du tableau en lui-même.
            {
                if (i < 9)
                    Console.Write(i + 1 + "  "); //À chaque début de ligne, on affiche le numéro de la ligne.
                else
                    Console.Write(i + 1 + " ");
                Console.Write("|"); //À chaque début de ligne, on dessine la bordure gauche du tableau.
                for (int k = 0; k < grille.GetLength(1); k++)
                {
                    if(grille[i,k].Length == 3) //Ici, il faut considérer l'éventualité qu'une des variables du tableau soit de longueur 3 (voir plus bas). On ne prend alors que les deux premiers caractères de la variable pour éviter la déformation du tableau (sachant que le 3ème est un espace).
                    {
                        CouleurGrille(grille[i, k]);
                        Console.Write(grille[i,k].Substring(0,2));
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("|"); //Après chaque affichage de variable, on dessine les bordures intérieures du tableau.
                    }
                    else
                    {
                        CouleurGrille(grille[i, k]);
                        Console.Write(grille[i, k]);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("|");
                    }
                }
                Console.WriteLine();
            }
            Console.Write("   ");
            for (int i = 0; i < 3 * grille.GetLength(1) + 1; i++) //Il s'agit de la même chose que la première boucle for mais pour la bordure inférieure du tableau.
            {
                Console.Write("-");
            }
            Console.WriteLine("\n");
        }
        
        static int CalculerDecompte(int ligneCible, int colonneCible, int[,] grilleJeu) //Cette fonction sert à calculer le décompte d'une case.
        {
            int decompte = 0; //On initialise le décompte à 0 pour ensuite lui ajouter les valeurs correspondantes.
            if(ligneCible > 0) //À chaque fois, on vérifie l'existence de la case que l'on veut utiliser.
            {
                //Ici, il s'agit de la case se trouvant au-dessus de celle dont on veut calculer le décompte :
                decompte += grilleJeu[ligneCible - 1, colonneCible]; //On ajoute à chaque fois la valeur 0 s'il n'y a rien, 1 s'il y a une mine et 2 s'il y a un trésor étant donné que l'on utilise la grille d'entiers comportant les mines et trésors.
            }
            if(ligneCible > 0 && colonneCible > 0)
            {
                //Ici il s'agit de la case se trouvant en haut à gauche de celle dont on veut calculer le décompte :
                decompte += grilleJeu[ligneCible - 1, colonneCible - 1];
            }
            if(colonneCible > 0)
            {
                decompte += grilleJeu[ligneCible, colonneCible - 1];
            }
            if(ligneCible < grilleJeu.GetLength(0)-1 && colonneCible > 0)
            {
                decompte += grilleJeu[ligneCible + 1, colonneCible - 1];
            }
            if(ligneCible < grilleJeu.GetLength(0) - 1)
            {
                decompte += grilleJeu[ligneCible + 1, colonneCible];
            }
            if(ligneCible < grilleJeu.GetLength(0) - 1 && colonneCible < grilleJeu.GetLength(1) - 1)
            {
                decompte += grilleJeu[ligneCible + 1, colonneCible + 1]; 
            }
            if(colonneCible < grilleJeu.GetLength(1) - 1)
            {
                decompte += grilleJeu[ligneCible, colonneCible + 1];
            }
            if(ligneCible > 0 && colonneCible < grilleJeu.GetLength(1) - 1)
            {
                decompte += grilleJeu[ligneCible - 1, colonneCible + 1];
            }
            return decompte;
        }

        static void AfficherDecompte(int ligneCible, int colonneCible, int[,] grilleJeu, string[,] grilleJoueur) //Cette fonction sert à modifier la grille joueur en lui intégrant les décomptes des cases autour de celle ciblée.
        {
            int decompte;
            if (ligneCible > 0 && grilleJoueur[ligneCible - 1, colonneCible] == "* ") //On vérifie toujours l'existence de la case à modifier et on vérifie aussi si elle a déjà été découverte ou non.
            {
                decompte = CalculerDecompte(ligneCible - 1, colonneCible, grilleJeu);
                grilleJoueur[ligneCible - 1, colonneCible] = decompte.ToString() + " ";
                /* On intégre à la case correspondante son décompte en le convertissant en string
                   et en laissant toujours un espace pour garder un string de longueur 2 afin de ne pas déformer le tableau.
                    On peut bien sûr avec ça, avoir un string de longueur 3 si le décompte est > 10 mais le problème est géré lors de
                    l'affichage de la grille.
                */
            }
            if (ligneCible > 0 && colonneCible > 0 && grilleJoueur[ligneCible - 1, colonneCible - 1] == "* ")
            {
                decompte = CalculerDecompte(ligneCible - 1, colonneCible - 1, grilleJeu);
                grilleJoueur[ligneCible - 1, colonneCible - 1] = decompte.ToString() + " ";
            }
            if (colonneCible > 0 && grilleJoueur[ligneCible, colonneCible - 1] == "* ")
            {
                decompte = CalculerDecompte(ligneCible, colonneCible - 1, grilleJeu);
                grilleJoueur[ligneCible, colonneCible - 1] = decompte.ToString() + " ";
            }
            if (ligneCible < grilleJeu.GetLength(0) - 1 && colonneCible > 0 && grilleJoueur[ligneCible + 1, colonneCible - 1] == "* ")
            {
                decompte = CalculerDecompte(ligneCible + 1, colonneCible - 1, grilleJeu);
                grilleJoueur[ligneCible + 1, colonneCible - 1] = decompte.ToString() + " ";
            }
            if (ligneCible < grilleJeu.GetLength(0) - 1 && grilleJoueur[ligneCible + 1, colonneCible] == "* ")
            {
                decompte = CalculerDecompte(ligneCible + 1, colonneCible, grilleJeu);
                grilleJoueur[ligneCible + 1, colonneCible] = decompte.ToString() + " ";
            }
            if (ligneCible < grilleJeu.GetLength(0) - 1 && colonneCible < grilleJeu.GetLength(1) - 1 && grilleJoueur[ligneCible + 1, colonneCible + 1] == "* ")
            {
                decompte = CalculerDecompte(ligneCible + 1, colonneCible + 1, grilleJeu);
                grilleJoueur[ligneCible + 1, colonneCible + 1] = decompte.ToString() + " ";
            }
            if (colonneCible < grilleJeu.GetLength(1) - 1 && grilleJoueur[ligneCible, colonneCible + 1] == "* ")
            {
                decompte = CalculerDecompte(ligneCible, colonneCible + 1, grilleJeu);
                grilleJoueur[ligneCible, colonneCible + 1] = decompte.ToString() + " ";
            }
            if (ligneCible > 0 && colonneCible < grilleJeu.GetLength(1) - 1 && grilleJoueur[ligneCible - 1, colonneCible + 1] == "* ")
            {
                decompte = CalculerDecompte(ligneCible - 1, colonneCible + 1, grilleJeu);
                grilleJoueur[ligneCible - 1, colonneCible + 1] = decompte.ToString() + " ";
            }
        }

        static int[] JouerCase(int nbLignesGrille, int nbColonnesGrille, string[,] grilleJoueur) //Cette fonction sert à demander quelle case veut découvrir le joueur à un tour lambda.
        {
            int ligneCible; //On initialise donc la ligne qu'il veut cibler.
            int colonneCible; //De même pour la colonne.
            do {
                bool conversion;
                do
                {
                    AfficherPhrase("Dans quelle ligne voulez-vous vous placer (entre 1 et " + nbLignesGrille + ") : ", false);
                    conversion = int.TryParse(Console.ReadLine(), out ligneCible);
                    if (ligneCible < 1 || ligneCible > nbLignesGrille || !conversion)
                        AfficherPhrase("Saisie incorrecte ! Vous devez saisir un entier entre 1 et " + nbLignesGrille + ".", true);
                }
                while (ligneCible < 1 || ligneCible > nbLignesGrille || !conversion); //Tant que le joueur saisit un chiffre ne correspondant à aucune ligne de la grille, on lui informe que la saisie est incorrect et on lui demande de recommencer la procédure. De même pour les caractères.

                do
                {
                    AfficherPhrase("Dans quelle colonne voulez-vous vous placer (entre 1 et " + nbColonnesGrille + ") : ", false);
                    conversion = int.TryParse(Console.ReadLine(), out colonneCible);
                    if (colonneCible < 1 || colonneCible > nbColonnesGrille || !conversion)
                        AfficherPhrase("Saisie incorrecte ! Vous devez saisir un entier entre 1 et " + nbLignesGrille + ".", true);
                }
                while (colonneCible < 1 || colonneCible > nbColonnesGrille || !conversion); //De même pour la colonne.

                if(grilleJoueur[ligneCible - 1, colonneCible - 1] != "* ")
                    AfficherPhrase("Cette case a déjà été découverte. Veuillez sélectionner une autre case.",true);

            } while(grilleJoueur[ligneCible - 1,colonneCible - 1] != "* "); //Tant que le joueur saisit une case qui a déjà été découverte, on lui redemande sa saisie.

            int[] place = new int[] {ligneCible - 1, colonneCible - 1}; //On stocke le jeu du joueur dans un tableau avec la ligne en 0 et la colonne en 1.

            return place;
        }

        static void Jouer(int ligneCible, int colonneCible, int [,] grilleJeu, string [,] grilleJoueur) //Cette fonction va permettre de mettre à jour la grille joueur en fonction du jeu du joueur.
        {
            if (grilleJeu[ligneCible, colonneCible] == 1) //Si la case selectionnée par le joueur est une mine, on note JM dans la grille joueur. J pour prendre connaissance de l'emplacement du joueur et M pour la mine.
            {
                grilleJoueur[ligneCible, colonneCible] = "JM";
            }

            else if (grilleJeu[ligneCible, colonneCible] == 2) //De même pour le trésor.
            {
                grilleJoueur[ligneCible, colonneCible] = "JT";
            }

            else //Il s'agit du cas où la case selectionnée n'est ni une mine ni un trésor.
            {
                int decompte = CalculerDecompte(ligneCible,colonneCible,grilleJeu); //On calcule le décompte de la case en question.
                if(decompte != 0) //Si son décompte est différent de 0, on l'affiche dans la grille du joueur.
                {
                    grilleJoueur[ligneCible,colonneCible] = decompte.ToString()+" ";
                } 
                else //Si le décompte = 0 :
                {
                    grilleJoueur[ligneCible,colonneCible] = "J "; //On affiche le joueur dans la case correspondante pour qu'il puisse se repérer.
                    AfficherDecompte(ligneCible, colonneCible, grilleJeu, grilleJoueur); //On affiche le décompte des cases entourant la case cible.
                    int compteur;
                    do //Dans cette boucle, on calcule le décompte des cases entourant celles dont le décompte = 0 tant qu'il y en a.
                    {
                        compteur = 0;
                        for (int i = 0; i < grilleJoueur.GetLength(0); i++)
                        {
                            for (int k = 0; k < grilleJoueur.GetLength(1); k++)
                            {
                                if (grilleJoueur[i, k] == "0 ")
                                {
                                    AfficherDecompte(i, k, grilleJeu, grilleJoueur);
                                    grilleJoueur[i, k] = "  "; //Dès qu'une case dont le décompte = 0 a été traitée, on la rend vide dans la grille du joueur.
                                    compteur++;
                                }
                            }
                        }
                    }
                    while (compteur != 0);
                }
            }

            CleanCase(ligneCible,colonneCible,grilleJoueur);
        }

        static void CleanCase(int ligneCible, int colonneCible, string[,] grilleJoueur) //Cette fonction permet d'effacer l'ancienne position du joueur lors d'un nouveau tour.
        {
            bool end = false;
            int i = 0;
            while (i < grilleJoueur.GetLength(0) && !end)
            {
                int k = 0;
                while (k < grilleJoueur.GetLength(1) && !end)
                {
                    if (grilleJoueur[i, k] == "J " && (i != ligneCible || k != colonneCible)) // Si la position du joueur était représentée par un "J " alors on remplace ce string par des espaces (il ne s'agissait ni d'une mine ni d'un trésor).
                    {
                        grilleJoueur[i, k] = "  ";
                        end = true; //Dès que l'on entre dans un if cela veut dire que l'on a detecté l'ancienne position du joueur : cela ne sert à rien de continuer la boucle.
                    }
                    else if (grilleJoueur[i, k] == "JT" && (i != ligneCible || k != colonneCible)) // Si la position du joueur était représentée par "JT", on remplace ce string par "T " puisqu'il s'agissait d'un trésor.
                    {
                        grilleJoueur[i, k] = "T ";
                        end = true;
                    }
                    k++;
                }
                i++;
            }
        }

        static bool IsGameOver(int ligneCible, int colonneCible, int[,] grilleJeu, string[,] grilleJoueur, int nbTresors, int nbMines) //Cette fonction sert à vérifier si la partie est terminée ou non.
        {
            bool gameOver = false; //On initie le booléen qui déterminera si la partie est terminée ou non.
            if (grilleJeu[ligneCible, colonneCible] == 1) //Si la case ciblée est une mine, alors la partie est terminée.
            {
                gameOver = true;
                Explosion();
            }
            else
            {
                int compteurTresors = 0; //On initialise un compteur afin de connaître le nombre de trésors découverts.
                int compteurCasesNonDecouvertes = 0; //On initialise un autre compteur afin de connaître le nombre de cases non découvertes.
                for (int i = 0; i < grilleJeu.GetLength(0); i++) //On parcourt la grille du joueur.
                {
                    for (int j = 0; j < grilleJeu.GetLength(1); j++)
                    {
                        if (grilleJoueur[i, j] == "JT" || grilleJoueur[i, j] == "T ") //Si la variable est un trésor alors on itére le compteur de trésors.
                            compteurTresors++;
                        if (grilleJoueur[i, j] == "* ") //Si la variable est une étoile, alors on itére le compteur de cases non découvertes.
                            compteurCasesNonDecouvertes++;
                    }
                }
                if (compteurTresors == nbTresors && compteurCasesNonDecouvertes == nbMines) //Si le nombre de trésors découverts est en fait le nombre de trésors et si le nombre de cases non découvertes est le nombre de mines, alors la partie est terminée.
                {
                    gameOver = true;
                    Victoire();
                }
            }
            return gameOver;
        }

        static void AfficherResultat(int ligneCible, int colonneCible, string[,] grilleJoueur, int[,] grilleJeu, int score) //Cette fonction sert à afficher les résultats de fin de partie.
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("==============================================");
            Console.WriteLine("               FIN DE PARTIE");
            Console.WriteLine("==============================================");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Votre grille :");
            AfficherGrilleJoueur(grilleJoueur);

            Console.WriteLine("La grille solution :");
            string[,] grilleSol = new string[grilleJeu.GetLength(0), grilleJeu.GetLength(1)]; //On convertit la grille jeu en string pour pouvoir utiliser la fonction AfficherGrilleJoueur.
            for(int i = 0; i < grilleJeu.GetLength(0); i++)
            {
                for(int k = 0; k < grilleJeu.GetLength(1); k++)
                {
                    if (grilleJeu[i, k] == 2)
                        grilleSol[i, k] = "T ";
                    else if (grilleJeu[i, k] == 1)
                        grilleSol[i, k] = "M ";
                    else
                        grilleSol[i, k] = "  ";
                }
            }
            AfficherGrilleJoueur(grilleSol);

            if (grilleJoueur[ligneCible, colonneCible] == "JM") //Si l'actuelle position du joueur est une mine, il a perdu. Sinon, il a gagné.
                AfficherPhrase("BOOM, tu t'es fait(e) explosé(e). C'est la DÉFAITE.", true);
            else
                AfficherPhrase("VICTOIRE !! T'as gagné mon gâté.", true);
            AfficherPhrase("Ton score est de : " + score + ".", true);
            Console.WriteLine();
        }

        static bool CommencerNouvellePartie() //Cette fonction sert à demander au joueur s'il veut relancer une nouvelle partie.
        {
            bool relancer = false;
            bool endLoop = false;
            do
            {
                AfficherPhrase("Voulez vous recommencer une partie ? Répondre par OUI ou NON : ", false);
                string recommencerPartie = Console.ReadLine().ToUpper(); //On convertit l'entrée du joueur en majuscule pour ne pas être gêner lors du traitement si le joueur écrit en minuscule.
                if (recommencerPartie == "OUI")
                {
                    Console.Clear();
                    relancer = true;
                    endLoop = true;
                }
                else if (recommencerPartie == "NON")
                {
                    relancer = false;
                    endLoop = true;
                }
                else
                    AfficherPhrase("Je n'ai pas compris votre saisie.", true);
            }
            while (!endLoop); //On réitére la boucle tant que le joueur n'a pas saisi correctement sa réponse.
            return relancer;
        }

        static void AfficherPhrase(string phrase, bool retourLigne) //Cette fonction sert à afficher les phrases caractère par caractère. Cela donne de l'esthétisme au jeu.
        {
            for(int i=0; i < phrase.Length; i++)
            {
                Console.Write(phrase[i]);
                Thread.Sleep(10); //Cela permet de mettre une pause de 10 millisecondes entre l'affichage de chaque caractère.
            }
            if(retourLigne) //Cette option que l'on ajoute dans les entrées de la fonction permet de faire un retour à la ligne ou non après l'affichage d'une phrase.
                Console.WriteLine();
        }

        static void CouleurGrille(string nombre) //Cette fonction permet de donner de la couleur à la grille lors de son affichage.
        {
            switch (nombre)
            {
                // Chaque même numéro lors de l'affichage des décomptes aura la même couleur.
                case "1 ":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "2 ":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case "3 ":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case "4 ":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case "5 ":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "6 ":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case "7 ":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "8 ":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "9 ":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "10 ":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "11 ":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "M ":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "T ":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }
        }

        static void Explosion() //Cette fonction permet de simuler une explosion lorsque le joueur tombe sur une mine.
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Thread.Sleep(20);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Thread.Sleep(20);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Thread.Sleep(20);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Thread.Sleep(20);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Thread.Sleep(20);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Thread.Sleep(20);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }

        static void Victoire() //Cette fonction permet d'afficher une animation lors de la victoire du joueur.
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();
            Thread.Sleep(200);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Clear();
            Thread.Sleep(200);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.WriteLine("██╗░░░██╗██╗░█████╗░████████╗░█████╗░██╗██████╗░███████╗  ██╗");
            Console.WriteLine("██║░░░██║██║██╔══██╗╚══██╔══╝██╔══██╗██║██╔══██╗██╔════╝  ██║");
            Console.WriteLine("╚██╗░██╔╝██║██║░░╚═╝░░░██║░░░██║░░██║██║██████╔╝█████╗░░  ██║");
            Console.WriteLine("░╚████╔╝░██║██║░░██╗░░░██║░░░██║░░██║██║██╔══██╗██╔══╝░░  ╚═╝");
            Console.WriteLine("░░╚██╔╝░░██║╚█████╔╝░░░██║░░░╚█████╔╝██║██║░░██║███████╗  ██╗");
            Console.WriteLine("░░░╚═╝░░░╚═╝░╚════╝░░░░╚═╝░░░░╚════╝░╚═╝╚═╝░░╚═╝╚══════╝  ╚═╝");

            Thread.Sleep(1000);
            Console.Clear();

            Console.WriteLine("███████╗███████╗██╗░░░░░██╗░█████╗░██╗████████╗░█████╗░████████╗██╗░█████╗░███╗░░██╗░██████╗░░░");
            Console.WriteLine("██╔════╝██╔════╝██║░░░░░██║██╔══██╗██║╚══██╔══╝██╔══██╗╚══██╔══╝██║██╔══██╗████╗░██║██╔════╝░░░");
            Console.WriteLine("█████╗░░█████╗░░██║░░░░░██║██║░░╚═╝██║░░░██║░░░███████║░░░██║░░░██║██║░░██║██╔██╗██║╚█████╗░░░░");
            Console.WriteLine("██╔══╝░░██╔══╝░░██║░░░░░██║██║░░██╗██║░░░██║░░░██╔══██║░░░██║░░░██║██║░░██║██║╚████║░╚═══██╗██╗");
            Console.WriteLine("██║░░░░░███████╗███████╗██║╚█████╔╝██║░░░██║░░░██║░░██║░░░██║░░░██║╚█████╔╝██║░╚███║██████╔╝╚█║");
            Console.WriteLine("╚═╝░░░░░╚══════╝╚══════╝╚═╝░╚════╝░╚═╝░░░╚═╝░░░╚═╝░░╚═╝░░░╚═╝░░░╚═╝░╚════╝░╚═╝░░╚══╝╚═════╝░░╚╝");

            Thread.Sleep(1000);
            Console.Clear();

            Console.WriteLine("████████╗██╗░░░██╗  ███████╗░██████╗  ██╗░░░██╗███╗░░██╗  ██╗░░░██╗██████╗░░█████╗░██╗");
            Console.WriteLine("╚══██╔══╝██║░░░██║  ██╔════╝██╔════╝  ██║░░░██║████╗░██║  ██║░░░██║██╔══██╗██╔══██╗██║");
            Console.WriteLine("░░░██║░░░██║░░░██║  █████╗░░╚█████╗░  ██║░░░██║██╔██╗██║  ╚██╗░██╔╝██████╔╝███████║██║");
            Console.WriteLine("░░░██║░░░██║░░░██║  ██╔══╝░░░╚═══██╗  ██║░░░██║██║╚████║  ░╚████╔╝░██╔══██╗██╔══██║██║");
            Console.WriteLine("░░░██║░░░╚██████╔╝  ███████╗██████╔╝  ╚██████╔╝██║░╚███║  ░░╚██╔╝░░██║░░██║██║░░██║██║");
            Console.WriteLine("░░░╚═╝░░░░╚═════╝░  ╚══════╝╚═════╝░  ░╚═════╝░╚═╝░░╚══╝  ░░░╚═╝░░░╚═╝░░╚═╝╚═╝░░╚═╝╚═╝");

            Thread.Sleep(1000);
            Console.Clear();

            Console.WriteLine("░█████╗░██╗░░░██╗███████╗███╗░░██╗████████╗██╗░░░██╗██████╗░██╗███████╗██████╗░  ██╗");
            Console.WriteLine("██╔══██╗██║░░░██║██╔════╝████╗░██║╚══██╔══╝██║░░░██║██╔══██╗██║██╔════╝██╔══██╗  ██║");
            Console.WriteLine("███████║╚██╗░██╔╝█████╗░░██╔██╗██║░░░██║░░░██║░░░██║██████╔╝██║█████╗░░██████╔╝  ██║");
            Console.WriteLine("██╔══██║░╚████╔╝░██╔══╝░░██║╚████║░░░██║░░░██║░░░██║██╔══██╗██║██╔══╝░░██╔══██╗  ╚═╝");
            Console.WriteLine("██║░░██║░░╚██╔╝░░███████╗██║░╚███║░░░██║░░░╚██████╔╝██║░░██║██║███████╗██║░░██║  ██╗");
            Console.WriteLine("╚═╝░░╚═╝░░░╚═╝░░░╚══════╝╚═╝░░╚══╝░░░╚═╝░░░░╚═════╝░╚═╝░░╚═╝╚═╝╚══════╝╚═╝░░╚═╝  ╚═╝");

            Thread.Sleep(2000);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
        }
    }
}
