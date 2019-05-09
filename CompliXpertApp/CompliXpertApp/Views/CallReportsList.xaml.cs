﻿using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CallReportsList : ContentPage
	{
		public CallReportsList (List<CallReport> callReports)
		{
			InitializeComponent ();
            BindingContext = new CallReportListViewModel(callReports);
		}
	}
}