﻿using System.Windows.Forms;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class FileDialogService : IFileDialogService
    {
        public string ShowFolderBrowserDialog()
        {
            string caminhoPasta = "";
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Selecione A Pasta";

            var result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                caminhoPasta = folderBrowserDialog.SelectedPath;
            }

            return caminhoPasta;
        }
    }
}
