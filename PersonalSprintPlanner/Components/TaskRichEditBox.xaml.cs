using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace PersonalSprintPlanner.Components
{
    public sealed partial class TaskRichEditBox : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set {
                string editorValue;
                Editor.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out editorValue);

                if (Text != value && value != editorValue)
                {
                    Editor.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, value);
                }
                SetValue(TextProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(RichEditBox), new PropertyMetadata(""));

        public TaskRichEditBox()
        {
            this.InitializeComponent();        
        }

        private void BulletsButton_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = Editor.Document.Selection;

            if (selectedText == null)
            {
                return;
            }

            ITextParagraphFormat paragraphFormatting = selectedText.ParagraphFormat;

            if (paragraphFormatting.ListType == MarkerType.Bullet)
            {
                paragraphFormatting.ListType = MarkerType.None;
            }
            else
            {
                paragraphFormatting.ListType = MarkerType.Bullet;
            }

            selectedText.ParagraphFormat = paragraphFormatting;
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = Editor.Document.Selection;

            if (selectedText == null)
            {
                return;
            }

            ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
            charFormatting.Bold = FormatEffect.Toggle;
            selectedText.CharacterFormat = charFormatting;
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = Editor.Document.Selection;

            if (selectedText == null)
            {
                return;
            }

            Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
            charFormatting.Italic = Windows.UI.Text.FormatEffect.Toggle;
            selectedText.CharacterFormat = charFormatting;
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = Editor.Document.Selection;

            if (selectedText == null)
            {
                return;
            }

            ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
            if (charFormatting.Underline == UnderlineType.None)
            {
                charFormatting.Underline = UnderlineType.Single;
            }
            else
            {
                charFormatting.Underline = UnderlineType.None;
            }
            selectedText.CharacterFormat = charFormatting;
        }

        private void Editor_TextChanged(object sender, RoutedEventArgs e)
        {
            string value;
            Editor.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out value);

            Text = value;
        }
    }
}
