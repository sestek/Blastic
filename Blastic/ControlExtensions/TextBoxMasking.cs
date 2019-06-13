using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Blastic.ControlExtensions
{
    public static class TextBoxMasking
    {
        public static readonly DependencyProperty MaskProperty = DependencyProperty.RegisterAttached(
	        nameof(MaskProperty).Replace("Property", ""),
	        typeof(string),
	        typeof(TextBoxMasking),
	        new FrameworkPropertyMetadata(OnMaskChanged));

        public static readonly DependencyProperty MaskExpressionProperty = DependencyProperty.RegisterAttached(
	        nameof(MaskExpressionProperty).Replace("Property", ""),
	        typeof(Regex),
	        typeof(TextBoxMasking));

        public static string GetMask(TextBox textBox)
        {
            return textBox.GetValue(MaskProperty) as string;
        }

        public static void SetMask(TextBox textBox, string mask)
        {
            textBox.SetValue(MaskProperty, mask);
        }

        private static Regex GetMaskExpression(TextBox textBox)
        {
            return textBox.GetValue(MaskExpressionProperty) as Regex;
        }

        private static void SetMaskExpression(TextBox textBox, Regex regex)
        {
            textBox.SetValue(MaskExpressionProperty, regex);
        }

        private static void OnMaskChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = dependencyObject as TextBox;
            string mask = e.NewValue as string;

            if (textBox == null)
            {
                return;
            }

            textBox.PreviewTextInput -= TextBox_PreviewTextInput;
            textBox.PreviewKeyDown -= TextBox_PreviewKeyDown;

            DataObject.RemovePastingHandler(textBox, Pasting);
            DataObject.RemoveCopyingHandler(textBox, NoDragCopy);
            CommandManager.RemovePreviewExecutedHandler(textBox, NoCutting);

            if (mask == null)
            {
                textBox.ClearValue(MaskProperty);
                textBox.ClearValue(MaskExpressionProperty);
            }
            else
            {
                textBox.SetValue(MaskProperty, mask);
                SetMaskExpression(textBox, new Regex(mask, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace));

                textBox.PreviewTextInput += TextBox_PreviewTextInput;
                textBox.PreviewKeyDown += TextBox_PreviewKeyDown;

                DataObject.AddPastingHandler(textBox, Pasting);
                DataObject.AddCopyingHandler(textBox, NoDragCopy);
                CommandManager.AddPreviewExecutedHandler(textBox, NoCutting);
            }
        }

        private static void NoCutting(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut)
            {
                e.Handled = true;
            }
        }

        private static void NoDragCopy(object sender, DataObjectCopyingEventArgs e)
        {
            if (e.IsDragDrop)
            {
                e.CancelCommand();
            }
        }

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Regex maskExpression = GetMaskExpression(textBox);

            if (maskExpression == null)
            {
                return;
            }

            string proposedText = GetProposedText(textBox, e.Text);

            if (!maskExpression.IsMatch(proposedText))
            {
                e.Handled = true;
            }
        }

        private static void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Regex maskExpression = GetMaskExpression(textBox);

            if (maskExpression == null)
            {
                return;
            }

            string proposedText = null;

            switch (e.Key)
            {
                case Key.Space:
                    proposedText = GetProposedText(textBox, " ");
                    break;
                case Key.Back:
                    proposedText = GetProposedTextBackspace(textBox);
                    break;
            }

            if (proposedText != null && !maskExpression.IsMatch(proposedText))
            {
                e.Handled = true;
            }

        }

        private static void Pasting(object sender, DataObjectPastingEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Regex maskExpression = GetMaskExpression(textBox);

            if (maskExpression == null)
            {
                return;
            }

            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pastedText = e.DataObject.GetData(typeof(string)) as string;
                string proposedText = GetProposedText(textBox, pastedText);

                if (!maskExpression.IsMatch(proposedText))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static string GetProposedTextBackspace(TextBox textBox)
        {
            string text = GetTextWithSelectionRemoved(textBox);
            if (textBox.SelectionStart > 0 && textBox.SelectionLength == 0)
            {
                text = text.Remove(textBox.SelectionStart - 1, 1);
            }

            return text;
        }


        private static string GetProposedText(TextBox textBox, string newText)
        {
            string text = GetTextWithSelectionRemoved(textBox);
            text = text.Insert(textBox.CaretIndex, newText);

            return text;
        }

        private static string GetTextWithSelectionRemoved(TextBox textBox)
        {
            string text = textBox.Text;

            if (textBox.SelectionStart != -1)
            {
                return text.Remove(textBox.SelectionStart, textBox.SelectionLength);
            }
            
            return text;
        }
    }
}