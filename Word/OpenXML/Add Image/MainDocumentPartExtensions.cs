using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;


namespace PZone.Samples
{
    public static class MainDocumentPartExtensions
    {
        public static ImagePart AddImagePart(this MainDocumentPart mainDocPart, Stream stream)
        {
            var imagePart = mainDocPart.AddImagePart(ImagePartType.Jpeg);
            imagePart.FeedData(stream);
            return imagePart;
        }


        public static Drawing CreateElement(this MainDocumentPart mainDocPart, ImagePart imagePart, string name, long width, long height, string horizontalAlignment = "left", long top = 0, long left = 0, long right = 0)
        {
            var relationshipId = mainDocPart.GetIdOfPart(imagePart);

            var yPos = top;
            var xPos = horizontalAlignment == "left" ? left : right;

            DW.Anchor anchor = new DW.Anchor();

            anchor.Append(
                new DW.SimplePosition() { X = xPos, Y = yPos }
            );

            anchor.Append(
                new DW.HorizontalPosition(new DW.HorizontalAlignment(horizontalAlignment))
                {
                    RelativeFrom = DW.HorizontalRelativePositionValues.Margin
                }
            );

            anchor.Append(
                new DW.VerticalPosition(new DW.PositionOffset("0"))
                {
                    RelativeFrom = DW.VerticalRelativePositionValues.Page // позиционирование относительно страницы
                }
            );

            anchor.Append(
                new DW.Extent { Cx = width, Cy = height }
            );

            anchor.Append(
                new DW.EffectExtent
                {
                    LeftEdge = 0L,
                    TopEdge = 0L,
                    RightEdge = 0L,
                    BottomEdge = 0L
                }
            );

            anchor.Append(new DW.WrapNone());

            anchor.Append(
                new DW.DocProperties
                {
                    Id = 1U,
                    Name = name
                }
            );

            anchor.Append(
                new DW.NonVisualGraphicFrameDrawingProperties(
                    new A.GraphicFrameLocks() { NoChangeAspect = true }
                )
            );

            anchor.Append(
                new A.Graphic(
                    new A.GraphicData(
                        new PIC.Picture(
                            new PIC.NonVisualPictureProperties(
                                new PIC.NonVisualDrawingProperties { Id = 0U, Name = name + ".jpg" },
                                new PIC.NonVisualPictureDrawingProperties()
                            ),
                            new PIC.BlipFill(
                                new A.Blip(
                                    new A.BlipExtensionList(
                                        new A.BlipExtension { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" }
                                    )
                                )
                                {
                                    Embed = relationshipId,
                                    CompressionState = A.BlipCompressionValues.Print
                                },
                                new A.Stretch(new A.FillRectangle())
                            ),
                            new PIC.ShapeProperties(
                                new A.Transform2D(
                                    new A.Offset { X = xPos, Y = yPos },
                                    new A.Extents { Cx = width, Cy = height }
                                ),
                                new A.PresetGeometry(new A.AdjustValueList())
                                {
                                    Preset = A.ShapeTypeValues.Rectangle
                                }
                            )
                        )
                    )
                    { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }
                )
            );

            anchor.DistanceFromTop = 0U;
            anchor.DistanceFromBottom = 0U;
            anchor.DistanceFromLeft = 114300U;
            anchor.DistanceFromRight = 114300U;
            anchor.SimplePos = false;
            anchor.RelativeHeight = 251679232U;
            anchor.BehindDoc = false;
            anchor.Locked = false;
            anchor.LayoutInCell = true;
            anchor.AllowOverlap = true;

            return new Drawing(anchor);
        }
    }
}