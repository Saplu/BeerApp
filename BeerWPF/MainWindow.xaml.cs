using IbuCalculations;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BeerWPF
{
    public partial class MainWindow : Window
    {
        private DataAccess _da;
        private Beer _beer;
        private Hop _hop;
        private BeerBitternessCalculator _calc;
        private List<Beer> _beers;
        private DispatcherTimer _timer;
        public MainWindow()
        {
            InitializeComponent();
            _da = new DataAccess();
            _beer = new Beer();
            _calc = new BeerBitternessCalculator(0, new List<Hop>()
            {
                new Hop() 
            });
            _beers = new List<Beer>();
            _hop = new Hop();
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 10);
            _timer.Tick += _timer_Tick;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BeerListBox.Items.Clear();
                _beers = _da.GetBeers();
                foreach (var item in _beers)
                {
                    BeerListBox.Items.Add(item.ToString());
                }
            }
            catch(Exception ex)
            {
                InfoLabel.Content = ex.Message;
                _timer.Start();
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
                InfoLabel.Content = "Beer added!";
                _timer.Start();
            }
            catch (Exception ex)
            {
                InfoLabel.Content = ex.Message;
                _timer.Start();
            }
        }

        private void HopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _beer.Hops.Add(_hop);
                _calc.Hops.Add(new Hop());
                _hop = new Hop();
                HopNameLabel.Text = null;
                AlphaLabel.Text = null;
                BoilLabel.Text = null;
                HopWeightLabel.Text = null;
                HopListBox.Items.Clear();
                foreach (var item in _beer.Hops)
                {
                    HopListBox.Items.Add(item.ToString());
                }
                InfoLabel.Content = "Hop added!";
                _timer.Start();
            }
            catch(Exception ex)
            {
                InfoLabel.Content = ex.Message;
                _timer.Start();
            }
        }

        private void BeerNameLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            _beer.Name = BeerNameLabel.Text;
        }

        private void AmountLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(VerifyDouble(AmountLabel.Text), out double value))
            {
                _beer.Amount = value;
                _calc.Volume = value;
                IbuLabel.Content = _calc.Bitterness().ToString();
            }
        }

        private void AlcoholLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(VerifyDouble(AlcoholLabel.Text), out double value))
                _beer.AlcoholPercentage = value;
        }

        private void DensityStartLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(VerifyDouble(DensityStartLabel.Text), out double value))
                _beer.DensityStart = value;
        }

        private void DensityEndLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(VerifyDouble(DensityEndLabel.Text), out double value))
                _beer.DensityEnd = value;
        }

        private void MaltLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(VerifyDouble(MaltLabel.Text), out double value))
                _beer.MaltExtractKg = value;
        }

        private void HopNameLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            _hop = new Hop();
            _hop.Name = HopNameLabel.Text;
            if (double.TryParse(VerifyDouble(AlphaLabel.Text), out double value))
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
            if (double.TryParse(VerifyDouble(AlphaLabel.Text), out double value))
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
            InfoLabel.Content = "Beer added!";
            _timer.Start();
            if (int.TryParse(HopWeightLabel.Text, out int value))
                _hop.Weight = value;
            else _hop.Weight = 0;
            IbuLabel.Content = _calc.Bitterness().ToString();
        }

        private void RemoveHop_Click(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count != 0)
                {
                    _beer.Hops.RemoveAt(HopListBox.SelectedIndex);
                    _calc.Hops.RemoveAt(HopListBox.SelectedIndex);
                    HopListBox.Items.Clear();
                    foreach (var item in _beer.Hops)
                    {
                        HopListBox.Items.Add(item);
                    }
                    IbuLabel.Content = _calc.Bitterness().ToString();
                }
            }
            catch(Exception ex)
            {
                InfoLabel.Content = ex.Message;
                _timer.Start();
            }
        }

        private void BeerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count != 0)
                {
                    _beer = _beers[BeerListBox.SelectedIndex];
                    BeerNameLabel.Text = _beer.Name;
                    AmountLabel.Text = _beer.Amount.ToString();
                    AlcoholLabel.Text = _beer.AlcoholPercentage.ToString();
                    DensityStartLabel.Text = _beer.DensityStart.ToString();
                    DensityEndLabel.Text = _beer.DensityEnd.ToString();
                    MaltLabel.Text = _beer.MaltExtractKg.ToString();
                    HopNameLabel.Text = null;
                    AlphaLabel.Text = null;
                    BoilLabel.Text = null;
                    HopWeightLabel.Text = null;
                    _calc.Volume = _beer.Amount;
                    HopListBox.Items.Clear();
                    foreach (var item in _beer.Hops)
                    {
                        _calc.Hops.Add(new Hop(item.Name, item.Weight, item.AlphaAcid, item.BoilingTime));
                        HopListBox.Items.Add(item);
                    }
                    _calc.Hops.Add(new Hop());
                }
            }
            catch(Exception ex)
            {
                InfoLabel.Content = ex.Message;
                _timer.Start();
            }
        }

        private string VerifyDouble(string value) => value.Replace('.', ',');

        private void _timer_Tick(object sender, EventArgs e)
        {
            InfoLabel.Content = "";
            _timer.Stop();
        }

        private void RemoveBeerButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Seriously delete selected beer?", "", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                var success = _da.RemoveBeer(_beer.Name);
                if (success == 0)
                {
                    InfoLabel.Content = "Beer successfully deleted :(";
                }
                else InfoLabel.Content = "No beer found to be deleted, than God.";
                _timer.Start();
            }
        }
    }
}
