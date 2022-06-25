using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proje1_Soru1
{
        

        internal class Program
        {
            static Random random = new Random();
            static void Main(string[] args)
            {
                int noktaSayisi = 20;
                int genislik = 100;
                int yükseklik = 100;

                double[,] noktaMatrisi = noktaMatrisiUret(noktaSayisi, genislik, yükseklik); //Her bir nokta için random x ve y değerleri üreten fonksiyon çağırıldı.
                                                                                             //Nokta matrisini yazdıran for döngüsü.
                for (int i = 0; i < noktaMatrisi.GetLength(0); i++)
                {
                    Console.WriteLine("x = {0}\ty = {1} ", noktaMatrisi[i, 0], noktaMatrisi[i, 1]);
                }

                //nxn'lik uzalık matrisi üreten fonksiyon. Her bir noktanın kendine ve diğer bütün noktalara uzaklığını içerir.
                double[,] uzaklıkMatrisi = uzaklıkMatrisiUret(noktaSayisi, noktaMatrisi);

                Console.Write("   ");
                for (int i = 0; i < noktaSayisi; i++)
                {
                    Console.Write(string.Format("{0,7}", i));

                }
                Console.WriteLine();

                Console.Write("     ");
                for (int i = 0; i < noktaSayisi; i++)
                {
                    Console.Write("_______");

                }
                Console.WriteLine();

                //uzaklık matrisini yazdıran for döngüsü.
                for (int i = 0; i < uzaklıkMatrisi.GetLength(0); i++)
                {
                    Console.Write(string.Format("{0,3}", i));
                    Console.Write(" |");

                    for (int j = 0; j < uzaklıkMatrisi.GetLength(1); j++)
                    {
                        Console.Write(string.Format("{0,7}", uzaklıkMatrisi[i, j].ToString("F2")));

                    }
                    Console.WriteLine();
                }

                //rastgele bir noktadan başlayarak tüm noktaları en yakın komşu yöntemine(nearest neighbor) göre dolaşan metodu çağırıldı.
                gezgin(uzaklıkMatrisi, noktaSayisi);
                Console.ReadKey();
            }
            /*Nokta matrisi üreten method. Her bir noktanın x ve y'e değerlerini parametre olarak gelen
            genişlik ve yükseklik değerlerine göre random olarak atar.*/
            static double[,] noktaMatrisiUret(int noktaSayisi, int parGenişlik, int parYükseklik)
            {
                double[,] matris = new double[noktaSayisi, 2];
                for (int i = 0; i < noktaSayisi; i++)
                {
                    double x = random.NextDouble() * parGenişlik;
                    double y = random.NextDouble() * parYükseklik;
                    matris[i, 0] = x;
                    matris[i, 1] = y;
                }
                return matris;
            }
            /*Uzaklık matrisi üreten method. Her bir noktanın kendisi ve diğer bütün noktalarla
            arasındaki uzaklıklardan nxn lik matris üretir.*/
            static double[,] uzaklıkMatrisiUret(int n, double[,] parMatris)
            {
                double[,] matris = new double[n, n];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        //Koordinat sisteminde iki nokta arasındaki mesafeyi veren formül.
                        matris[i, j] = Math.Sqrt(Math.Pow(parMatris[i, 0] - parMatris[j, 0], 2) +
                            (Math.Pow(parMatris[i, 1] - parMatris[j, 1], 2)));
                    }
                }
                return matris;
            }

            static void gezgin(double[,] parUzaklıkMatrisi, int n)
            {
                ArrayList başlangıçNoktaları = new ArrayList(10); //Rastgele 1o farklı noktadan başlamak için başlanan noktaları tutan array list.
                ArrayList gidilenYerler = new ArrayList(n); //Bir turda gidilen noktaları, bir daha gitmemek için tutan arraylit

                int başlangıçNoktası;
                object minNokta;
                double toplamYol;
                //10 kez (10tur) dönen for metodu
                for (int turSayısı = 1; turSayısı < 11; turSayısı++)
                {
                    toplamYol = 0; //Bir turda gidilne toplam yolu tutan değişken.
                    başlangıçNoktası = random.Next(n); //başlangıç noktası her turun başında random olarak atanır.
                                                       //Eğer random olarak atanan başlangıç noktasından daha önce başlandıysa tekrar atanacak while döngüsü.
                    while (başlangıçNoktaları.Contains(başlangıçNoktası) == true)
                    {
                        başlangıçNoktası = random.Next(n);
                    }
                    başlangıçNoktaları.Add(başlangıçNoktası);//Başlangıç noktası bir daha atanmaması için arrayliste eklenmiştir.
                    gidilenYerler.Add(başlangıçNoktası);//O turda gidilen ilk nokta başlangıç noktasıdır.
                    ArrayList anlıkIndex = new ArrayList(n);//Üzerinde durulan noktanın diğer noktalarla uzaklığını tutan arraylist.

                    for (int j = 0; j < n; j++)//başlangıçtaki noktanın diğer noktalarla uzaklığı anlıkIndex listesine eklenmiştir.
                    {
                        anlıkIndex.Insert(j, parUzaklıkMatrisi[başlangıçNoktası, j]);
                    }
                    for (int i = 1; i < n; i++)
                    {
                        int a = 0;
                        minNokta = anlıkIndex[a];//En yakın nokta ilk başta ilk eleman olarak atanmıştır.
                                                 //Fakat en yakın nokta kendisi ve daha önce gidilen nokta olarak başlangıçta atnamaz.
                                                 //Bu şartları sağlayana kadar dönecek while döngüsü.
                        while (minNokta.Equals(0) || gidilenYerler.Contains(anlıkIndex.IndexOf(minNokta)))
                        {
                            a++;
                            minNokta = anlıkIndex[a];
                        }
                        //İlk min değeri belirlendikten sonra diğer noktalara da bakılarak  şartları sağlayan en yakın nokta bulunur.
                        foreach (double item in anlıkIndex)
                        {
                            if ((int)item == 0)//Kendisine gidemez.
                                continue;
                            else if (gidilenYerler.Contains(anlıkIndex.IndexOf(item)))//Daha önce gittiği noktaya gidemez.
                                continue;
                            else
                            {
                                if (item.CompareTo(minNokta) == -1)
                                    minNokta = item; //Şartları sağlayan en yakın nokta belirlenmiştir.
                            }
                        }
                        int yeniNokta = anlıkIndex.IndexOf(minNokta); //Bir sonraki gidilecek nokta belirlenmiştir.
                        gidilenYerler.Add(anlıkIndex.IndexOf(minNokta));//Bir sonraki gidilecek nokta gidilen noktalar listesine eklenmiştir.
                        toplamYol += (double)minNokta; //Gidilen noktanın uzaklığı toplam yola eklenmiştir.

                        //Gidilen yeni noktanın diğer noktalarla uzaklığını tutan anlıkIndex listesi güncellenmiştir.
                        anlıkIndex.Clear();
                        for (int j = 0; j < n; j++)
                        {
                            anlıkIndex.Insert(j, parUzaklıkMatrisi[yeniNokta, j]);
                        }
                    }
                    Console.WriteLine();
                    //Bu turda gidilen noktalar sırasıyla yazdırılmıştır.
                    Console.WriteLine("Tur {0}:", turSayısı);
                    foreach (object item in gidilenYerler)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();

                    gidilenYerler.Clear();//Gidilen noktaları tutan liste bir sonraki tur için sıfırlanmıştır.
                    Console.WriteLine("Toplam yol uzunluğu: {0:0.00} ", toplamYol);//O turda gidilen toplam yol yazdırıldı.
                }
            }
        }
    }

