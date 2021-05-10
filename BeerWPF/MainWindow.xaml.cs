﻿using IbuCalculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataAccess _da;
        private Beer _beer;
        private Hop _hop;
        private BeerBitternessCalculator _calc;
        public MainWindow()
        {
            InitializeComponent();
            _da = new DataAccess();
            _beer = new Beer();
            _calc = new BeerBitternessCalculator(0, new List<Hop>()
            {
                new Hop() 
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BeerListBox.Items.Clear();
            var beers = _da.GetBeers();
            foreach(var item in beers)
            {
                BeerListBox.Items.Add(item.ToString());
            }
        }

        private void SaveBeerButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _beer.Ibu = Convert.ToInt32(_calc.Bitterness());
                _da.UpsertBeer(_beer);
                _beer = new Beer();
                _calc = new BeerBitternessCalculator(0, new List<Hop>()
                {
                    new Hop()
                });
                IbuLabel.Content = 0;
                foreach(var item in MainGrid.Children)
                {
                    if (item.GetType() == typeof(TextBox))
                        ((TextBox)item).Text = null;
                }
                HopListBox.Items.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HopButton_Click(object sender, RoutedEventArgs e)
        {
            _beer.Hops.Add(_hop);
            _calc.Hops.Add(new Hop());
            _hop = new Hop();
            HopNameLabel.Text = null;
            AlphaLabel.Text = null;
            BoilLabel.Text = null;
            HopWeightLabel.Text = null;
            HopListBox.Items.Clear();
            foreach(var item in _beer.Hops)
            {
                HopListBox.Items.Add(item.ToString());
            }
            var asd = _beer;
        }

        private void BeerNameLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            _beer.Name = BeerNameLabel.Text;
        }

        private void AmountLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(AmountLabel.Text, out double value))
            {
                _beer.Amount = value;
                _calc.Volume = value;
                IbuLabel.Content = _calc.Bitterness().ToString();
            }
        }

        private void AlcoholLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(AlcoholLabel.Text, out double value))
                _beer.AlcoholPercentage = value;
        }

        private void DensityStartLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(DensityStartLabel.Text, out double value))
                _beer.DensityStart = value;
        }

        private void DensityEndLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(DensityEndLabel.Text, out double value))
                _beer.DensityEnd = value;
        }

        private void MaltLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(MaltLabel.Text, out double value))
                _beer.MaltExtractKg = value;
        }

        private void HopNameLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            _hop = new Hop();
            _hop.Name = HopNameLabel.Text;
            if (double.TryParse(AlphaLabel.Text, out double value))
                _hop.AlphaAcid = value;
            if (int.TryParse(BoilLabel.Text, out int boil))
                _hop.BoilingTime = boil;
            if (int.TryParse(HopWeightLabel.Text, out int weight))
                _hop.Weight = weight;
            _calc.Hops[_calc.Hops.Count - 1] = _hop;
            IbuLabel.Content = _calc.Bitterness().ToString();
        }

        private void AlphaLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(AlphaLabel.Text, out double value))
                _hop.AlphaAcid = value;
            else _hop.AlphaAcid = 0;
            IbuLabel.Content = _calc.Bitterness().ToString();
        }

        private void BoilLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(BoilLabel.Text, out int value))
                _hop.BoilingTime = value;
            else _hop.BoilingTime = 0;
            IbuLabel.Content = _calc.Bitterness().ToString();
        }

        private void HopWeightLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(HopWeightLabel.Text, out int value))
                _hop.Weight = value;
            else _hop.Weight = 0;
            IbuLabel.Content = _calc.Bitterness().ToString();
        }

        private void RemoveHop_Click(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                _beer.Hops.RemoveAt(HopListBox.SelectedIndex);
                _calc.Hops.RemoveAt(HopListBox.SelectedIndex);
                HopListBox.Items.RemoveAt(HopListBox.SelectedIndex);
                IbuLabel.Content = _calc.Bitterness().ToString();
            }
        }
    }
}
