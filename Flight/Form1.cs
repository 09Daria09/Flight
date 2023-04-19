using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Flight
{
    public partial class Form1 : Form
    {
        Flight[] flights = null;
        List<Passenger> passengers;
        public Form1()
        {
            InitializeComponent();

            Flight flight1 = new Flight("SU123", new DateTime(2023, 05, 10, 14, 30, 00), TimeSpan.FromHours(3.5), "Нью-Йорк");
            Flight flight2 = new Flight("FR456", new DateTime(2023, 05, 12, 10, 15, 00), TimeSpan.FromHours(2.5), "Лондон");
            Flight flight3 = new Flight("EK789", new DateTime(2023, 05, 15, 18, 20, 00), TimeSpan.FromHours(5), "Дубай");

            flights = new Flight[] { flight1, flight2, flight3 };

            comboBox1.Items.Add(flight1.NumberFlight);
            comboBox1.Items.Add(flight2.NumberFlight);
            comboBox1.Items.Add(flight3.NumberFlight);
            passengers = new List<Passenger>()
{
    new Passenger("Иванов Иван Иванович", 2, 30, flight1),
    new Passenger("Петров Петр Петрович", 1, 20, flight1),
    new Passenger("Сидоров Сидор Сидорович", 3, 45, flight1),
    new Passenger("Козлов Константин Игоревич", 1, 20, flight1),
    new Passenger("Иванова Елена Сергеевна", 2, 30, flight2),
    new Passenger("Петрова Анна Александровна", 1, 15, flight2),
    new Passenger("Сидорова Наталья Алексеевна", 2, 25, flight2),
    new Passenger("Козлова Екатерина Ивановна", 1, 20, flight2),
    new Passenger("Николаев Игорь Владимирович", 2, 35, flight3),
    new Passenger("Федорова Мария Петровна", 1, 15, flight3)
};

            listBox1.Items.AddRange(passengers.Select(p => $"{p.FML} | {p.flight.NumberFlight} | {p.flight.DepartureDate:dd.MM.yyyy} | {p.flight.Destination}").ToArray());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string flightNumber = (sender as ComboBox).SelectedItem.ToString();

            foreach (Flight flight in flights)
            {
                if (flight.NumberFlight == flightNumber)
                {
                    textBox1.Text = flight.DepartureDate.ToShortDateString();
                    textBox2.Text = flight.DepartureDate.ToShortTimeString();
                    textBox3.Text = flight.FlightDuration.ToString();
                    textBox4.Text = flight.Destination;
                    break;
                }
            }
            listBox1.Items.Clear();
            for (int i = 0; i <= passengers.Count - 1; i++)
            {
                if (flightNumber == passengers[i].flight.NumberFlight)
                {
                    listBox1.Items.Add(($"{passengers[i].FML} | {passengers[i].flight.NumberFlight} | {passengers[i].flight.DepartureDate:dd.MM.yyyy} | {passengers[i].flight.Destination}").ToString());
                }

            }

            Thread thread = new Thread(WriteToFile);

            thread.Start();

            thread.Join();

            Console.WriteLine("Запись в файл завершена.");
        }

        private void WriteToFile()
        {
            double AllLuggage = 0;
            string selectedItem = listBox1.Items[0].ToString();
            string[] parts = selectedItem.Split('|');
            string result = parts[1].Trim();
            string plase = null;
            DateTime arrivalDate = new DateTime(2023, 4, 19);

            using (StreamWriter writer = new StreamWriter("Flight.txt"))
            {
                writer.WriteLine($"\t\tРейс - {comboBox1.Text}\nПассажиры:");
                foreach (var item in listBox1.Items)
                {
                    writer.WriteLine(item.ToString());
                }
                for (int i = 0; i <= passengers.Count - 1; i++)
                {
                    if (passengers[i].flight.NumberFlight == result)
                    {
                        AllLuggage += passengers[i].WeightLuggage;
                        arrivalDate = passengers[i].flight.DepartureDate + passengers[i].flight.FlightDuration;
                        plase = passengers[i].flight.Destination;
                    }
                }
                writer.WriteLine($"Суммарный вес багажа: {AllLuggage}");

                writer.WriteLine($"Дата и время прибытия: {arrivalDate} ");
                writer.WriteLine($"Место прибытия: {plase}");
            }
        }
    }
    class Flight
    {
        public string NumberFlight { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan FlightDuration { get; set; }
        public string Destination { get; set; }

        public Flight(string numberFlight, DateTime date, TimeSpan time, string destination)
        {
            NumberFlight = numberFlight;
            DepartureDate = date;
            FlightDuration = time;
            Destination = destination;
        }
    }

    class Passenger
    {
        public string FML { get; set; }
        public int CountLuggage { get; set; }
        public int WeightLuggage { get; set; }
        public Flight flight { get; set; }
        public Passenger(string fml, int countLuggage, int weightLuggage, Flight _flight)
        {
            FML = fml;
            CountLuggage = countLuggage;
            WeightLuggage = weightLuggage;
            flight = _flight;
        }
    }
}