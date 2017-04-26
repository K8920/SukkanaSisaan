using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace SukkanaSisaan
{
    class Audio
    {
        public MediaElement mediaElement;
        public MediaElement mediaElement2;
        public async void InitAudio()
        {
            mediaElement = new MediaElement();
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync("grass.mp3");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            mediaElement.AutoPlay = true;
            mediaElement.SetSource(stream, file.ContentType);
        }

       //private async void InitAudio_2()
       //{
       //    mediaElement_2 = new MediaElement();
       //    StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
       //    StorageFile file = await folder.GetFileAsync("grass.mp3");
       //    var stream = await file.OpenAsync(FileAccessMode.Read);
       //    mediaElement_2.AutoPlay = true;
       //    mediaElement_2.SetSource(stream, file.ContentType);
       //}
    }
}
