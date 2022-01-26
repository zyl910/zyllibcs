using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using zyllibcs.text;
using System.Reflection;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;

namespace zinfoimage {
    /// <summary>
    /// My info: Image infos.
    /// </summary>
    class MyInfo {
        /// <summary>File path.</summary>
        public static string FilePath { get; set; }
        /// <summary>Show bytes.</summary>
        public static bool ShowBytes { get; set; }
        /// <summary>Show Compression.</summary>
        public static bool ShowCompression = true;

        /// <summary>IMAGE_COMPRESSION_ Map</summary>
        private static readonly Dictionary<int, string> IMAGE_COMPRESSION_Map = new Dictionary<int, string>{
            {1, "UNCOMPRESSED"},
            {2, "CCITT_T3"},
            {3, "CCITT_T4"},
            {4, "CCITT_T6"},
            {5, "LZW"},
            {6, "JPEG"}
        };

        // https://docs.microsoft.com/en-us/windows/win32/gdiplus/-gdiplus-constant-property-tags-in-numerical-order
        // https://docs.microsoft.com/zh-cn/dotnet/api/system.drawing.imaging.propertyitem.id?view=dotnet-plat-ext-6.0
        /// <summary>The PropertyId name map</summary>
        private static readonly Dictionary<int, string> PropertyIdMap = new Dictionary<int, string>{
            {0x0000, "PropertyTagGpsVer"},
            {0x0001, "PropertyTagGpsLatitudeRef"},
            {0x0002, "PropertyTagGpsLatitude"},
            {0x0003, "PropertyTagGpsLongitudeRef"},
            {0x0004, "PropertyTagGpsLongitude"},
            {0x0005, "PropertyTagGpsAltitudeRef"},
            {0x0006, "PropertyTagGpsAltitude"},
            {0x0007, "PropertyTagGpsGpsTime"},
            {0x0008, "PropertyTagGpsGpsSatellites"},
            {0x0009, "PropertyTagGpsGpsStatus"},
            {0x000A, "PropertyTagGpsGpsMeasureMode"},
            {0x000B, "PropertyTagGpsGpsDop"},
            {0x000C, "PropertyTagGpsSpeedRef"},
            {0x000D, "PropertyTagGpsSpeed"},
            {0x000E, "PropertyTagGpsTrackRef"},
            {0x000F, "PropertyTagGpsTrack"},
            {0x0010, "PropertyTagGpsImgDirRef"},
            {0x0011, "PropertyTagGpsImgDir"},
            {0x0012, "PropertyTagGpsMapDatum"},
            {0x0013, "PropertyTagGpsDestLatRef"},
            {0x0014, "PropertyTagGpsDestLat"},
            {0x0015, "PropertyTagGpsDestLongRef"},
            {0x0016, "PropertyTagGpsDestLong"},
            {0x0017, "PropertyTagGpsDestBearRef"},
            {0x0018, "PropertyTagGpsDestBear"},
            {0x0019, "PropertyTagGpsDestDistRef"},
            {0x001A, "PropertyTagGpsDestDist"},
            {0x00FE, "PropertyTagNewSubfileType"},
            {0x00FF, "PropertyTagSubfileType"},
            {0x0100, "PropertyTagImageWidth"},
            {0x0101, "PropertyTagImageHeight"},
            {0x0102, "PropertyTagBitsPerSample"},
            {0x0103, "PropertyTagCompression"},
            {0x0106, "PropertyTagPhotometricInterp"},
            {0x0107, "PropertyTagThreshHolding"},
            {0x0108, "PropertyTagCellWidth"},
            {0x0109, "PropertyTagCellHeight"},
            {0x010A, "PropertyTagFillOrder"},
            {0x010D, "PropertyTagDocumentName"},
            {0x010E, "PropertyTagImageDescription"},
            {0x010F, "PropertyTagEquipMake"},
            {0x0110, "PropertyTagEquipModel"},
            {0x0111, "PropertyTagStripOffsets"},
            {0x0112, "PropertyTagOrientation"},
            {0x0115, "PropertyTagSamplesPerPixel"},
            {0x0116, "PropertyTagRowsPerStrip"},
            {0x0117, "PropertyTagStripBytesCount"},
            {0x0118, "PropertyTagMinSampleValue"},
            {0x0119, "PropertyTagMaxSampleValue"},
            {0x011A, "PropertyTagXResolution"},
            {0x011B, "PropertyTagYResolution"},
            {0x011C, "PropertyTagPlanarConfig"},
            {0x011D, "PropertyTagPageName"},
            {0x011E, "PropertyTagXPosition"},
            {0x011F, "PropertyTagYPosition"},
            {0x0120, "PropertyTagFreeOffset"},
            {0x0121, "PropertyTagFreeByteCounts"},
            {0x0122, "PropertyTagGrayResponseUnit"},
            {0x0123, "PropertyTagGrayResponseCurve"},
            {0x0124, "PropertyTagT4Option"},
            {0x0125, "PropertyTagT6Option"},
            {0x0128, "PropertyTagResolutionUnit"},
            {0x0129, "PropertyTagPageNumber"},
            {0x012D, "PropertyTagTransferFunction"},
            {0x0131, "PropertyTagSoftwareUsed"},
            {0x0132, "PropertyTagDateTime"},
            {0x013B, "PropertyTagArtist"},
            {0x013C, "PropertyTagHostComputer"},
            {0x013D, "PropertyTagPredictor"},
            {0x013E, "PropertyTagWhitePoint"},
            {0x013F, "PropertyTagPrimaryChromaticities"},
            {0x0140, "PropertyTagColorMap"},
            {0x0141, "PropertyTagHalftoneHints"},
            {0x0142, "PropertyTagTileWidth"},
            {0x0143, "PropertyTagTileLength"},
            {0x0144, "PropertyTagTileOffset"},
            {0x0145, "PropertyTagTileByteCounts"},
            {0x014C, "PropertyTagInkSet"},
            {0x014D, "PropertyTagInkNames"},
            {0x014E, "PropertyTagNumberOfInks"},
            {0x0150, "PropertyTagDotRange"},
            {0x0151, "PropertyTagTargetPrinter"},
            {0x0152, "PropertyTagExtraSamples"},
            {0x0153, "PropertyTagSampleFormat"},
            {0x0154, "PropertyTagSMinSampleValue"},
            {0x0155, "PropertyTagSMaxSampleValue"},
            {0x0156, "PropertyTagTransferRange"},
            {0x0200, "PropertyTagJPEGProc"},
            {0x0201, "PropertyTagJPEGInterFormat"},
            {0x0202, "PropertyTagJPEGInterLength"},
            {0x0203, "PropertyTagJPEGRestartInterval"},
            {0x0205, "PropertyTagJPEGLosslessPredictors"},
            {0x0206, "PropertyTagJPEGPointTransforms"},
            {0x0207, "PropertyTagJPEGQTables"},
            {0x0208, "PropertyTagJPEGDCTables"},
            {0x0209, "PropertyTagJPEGACTables"},
            {0x0211, "PropertyTagYCbCrCoefficients"},
            {0x0212, "PropertyTagYCbCrSubsampling"},
            {0x0213, "PropertyTagYCbCrPositioning"},
            {0x0214, "PropertyTagREFBlackWhite"},
            {0x0301, "PropertyTagGamma"},
            {0x0302, "PropertyTagICCProfileDescriptor"},
            {0x0303, "PropertyTagSRGBRenderingIntent"},
            {0x0320, "PropertyTagImageTitle"},
            {0x5001, "PropertyTagResolutionXUnit"},
            {0x5002, "PropertyTagResolutionYUnit"},
            {0x5003, "PropertyTagResolutionXLengthUnit"},
            {0x5004, "PropertyTagResolutionYLengthUnit"},
            {0x5005, "PropertyTagPrintFlags"},
            {0x5006, "PropertyTagPrintFlagsVersion"},
            {0x5007, "PropertyTagPrintFlagsCrop"},
            {0x5008, "PropertyTagPrintFlagsBleedWidth"},
            {0x5009, "PropertyTagPrintFlagsBleedWidthScale"},
            {0x500A, "PropertyTagHalftoneLPI"},
            {0x500B, "PropertyTagHalftoneLPIUnit"},
            {0x500C, "PropertyTagHalftoneDegree"},
            {0x500D, "PropertyTagHalftoneShape"},
            {0x500E, "PropertyTagHalftoneMisc"},
            {0x500F, "PropertyTagHalftoneScreen"},
            {0x5010, "PropertyTagJPEGQuality"},
            {0x5011, "PropertyTagGridSize"},
            {0x5012, "PropertyTagThumbnailFormat"},
            {0x5013, "PropertyTagThumbnailWidth"},
            {0x5014, "PropertyTagThumbnailHeight"},
            {0x5015, "PropertyTagThumbnailColorDepth"},
            {0x5016, "PropertyTagThumbnailPlanes"},
            {0x5017, "PropertyTagThumbnailRawBytes"},
            {0x5018, "PropertyTagThumbnailSize"},
            {0x5019, "PropertyTagThumbnailCompressedSize"},
            {0x501A, "PropertyTagColorTransferFunction"},
            {0x501B, "PropertyTagThumbnailData"},
            {0x5020, "PropertyTagThumbnailImageWidth"},
            {0x5021, "PropertyTagThumbnailImageHeight"},
            {0x5022, "PropertyTagThumbnailBitsPerSample"},
            {0x5023, "PropertyTagThumbnailCompression"},
            {0x5024, "PropertyTagThumbnailPhotometricInterp"},
            {0x5025, "PropertyTagThumbnailImageDescription"},
            {0x5026, "PropertyTagThumbnailEquipMake"},
            {0x5027, "PropertyTagThumbnailEquipModel"},
            {0x5028, "PropertyTagThumbnailStripOffsets"},
            {0x5029, "PropertyTagThumbnailOrientation"},
            {0x502A, "PropertyTagThumbnailSamplesPerPixel"},
            {0x502B, "PropertyTagThumbnailRowsPerStrip"},
            {0x502C, "PropertyTagThumbnailStripBytesCount"},
            {0x502D, "PropertyTagThumbnailResolutionX"},
            {0x502E, "PropertyTagThumbnailResolutionY"},
            {0x502F, "PropertyTagThumbnailPlanarConfig"},
            {0x5030, "PropertyTagThumbnailResolutionUnit"},
            {0x5031, "PropertyTagThumbnailTransferFunction"},
            {0x5032, "PropertyTagThumbnailSoftwareUsed"},
            {0x5033, "PropertyTagThumbnailDateTime"},
            {0x5034, "PropertyTagThumbnailArtist"},
            {0x5035, "PropertyTagThumbnailWhitePoint"},
            {0x5036, "PropertyTagThumbnailPrimaryChromaticities"},
            {0x5037, "PropertyTagThumbnailYCbCrCoefficients"},
            {0x5038, "PropertyTagThumbnailYCbCrSubsampling"},
            {0x5039, "PropertyTagThumbnailYCbCrPositioning"},
            {0x503A, "PropertyTagThumbnailRefBlackWhite"},
            {0x503B, "PropertyTagThumbnailCopyRight"},
            {0x5090, "PropertyTagLuminanceTable"},
            {0x5091, "PropertyTagChrominanceTable"},
            {0x5100, "PropertyTagFrameDelay"},
            {0x5101, "PropertyTagLoopCount"},
            {0x5102, "PropertyTagGlobalPalette"},
            {0x5103, "PropertyTagIndexBackground"},
            {0x5104, "PropertyTagIndexTransparent"},
            {0x5110, "PropertyTagPixelUnit"},
            {0x5111, "PropertyTagPixelPerUnitX"},
            {0x5112, "PropertyTagPixelPerUnitY"},
            {0x5113, "PropertyTagPaletteHistogram"},
            {0x8298, "PropertyTagCopyright"},
            {0x829A, "PropertyTagExifExposureTime"},
            {0x829D, "PropertyTagExifFNumber"},
            {0x8769, "PropertyTagExifIFD"},
            {0x8773, "PropertyTagICCProfile"},
            {0x8822, "PropertyTagExifExposureProg"},
            {0x8824, "PropertyTagExifSpectralSense"},
            {0x8825, "PropertyTagGpsIFD"},
            {0x8827, "PropertyTagExifISOSpeed"},
            {0x8828, "PropertyTagExifOECF"},
            {0x9000, "PropertyTagExifVer"},
            {0x9003, "PropertyTagExifDTOrig"},
            {0x9004, "PropertyTagExifDTDigitized"},
            {0x9101, "PropertyTagExifCompConfig"},
            {0x9102, "PropertyTagExifCompBPP"},
            {0x9201, "PropertyTagExifShutterSpeed"},
            {0x9202, "PropertyTagExifAperture"},
            {0x9203, "PropertyTagExifBrightness"},
            {0x9204, "PropertyTagExifExposureBias"},
            {0x9205, "PropertyTagExifMaxAperture"},
            {0x9206, "PropertyTagExifSubjectDist"},
            {0x9207, "PropertyTagExifMeteringMode"},
            {0x9208, "PropertyTagExifLightSource"},
            {0x9209, "PropertyTagExifFlash"},
            {0x920A, "PropertyTagExifFocalLength"},
            {0x927C, "PropertyTagExifMakerNote"},
            {0x9286, "PropertyTagExifUserComment"},
            {0x9290, "PropertyTagExifDTSubsec"},
            {0x9291, "PropertyTagExifDTOrigSS"},
            {0x9292, "PropertyTagExifDTDigSS"},
            {0xA000, "PropertyTagExifFPXVer"},
            {0xA001, "PropertyTagExifColorSpace"},
            {0xA002, "PropertyTagExifPixXDim"},
            {0xA003, "PropertyTagExifPixYDim"},
            {0xA004, "PropertyTagExifRelatedWav"},
            {0xA005, "PropertyTagExifInterop"},
            {0xA20B, "PropertyTagExifFlashEnergy"},
            {0xA20C, "PropertyTagExifSpatialFR"},
            {0xA20E, "PropertyTagExifFocalXRes"},
            {0xA20F, "PropertyTagExifFocalYRes"},
            {0xA210, "PropertyTagExifFocalResUnit"},
            {0xA214, "PropertyTagExifSubjectLoc"},
            {0xA215, "PropertyTagExifExposureIndex"},
            {0xA217, "PropertyTagExifSensingMethod"},
            {0xA300, "PropertyTagExifFileSource"},
            {0xA301, "PropertyTagExifSceneType"},
            {0xA302, "PropertyTagExifCfaPattern"}
        };

        /// <summary>
        /// FrameDimension Map.
        /// </summary>
        private static IDictionary<Guid, KeyValuePair<string, FrameDimension>> FrameDimensionMap = new Dictionary<Guid, KeyValuePair<string, FrameDimension>>{
            {FrameDimension.Page.Guid, new KeyValuePair<string, FrameDimension>("Page", FrameDimension.Page)},
            {FrameDimension.Resolution.Guid, new KeyValuePair<string, FrameDimension>("Resolution", FrameDimension.Resolution)},
            {FrameDimension.Time.Guid, new KeyValuePair<string, FrameDimension>("Time", FrameDimension.Time)},
        };

        /// <summary>
        /// 输出多行_图像编码参数.
        /// </summary>
        /// <param name="iw">带缩进输出者.</param>
        /// <param name="obj">object. Can be null.</param>
        /// <param name="context">State Object. Can be null.</param>
        /// <returns>返回是否成功输出.</returns>
        public static bool outl_GetEncoderParameterList(IIndentedWriter iw, Image obj, IndentedWriterContext context) {
            if (null == iw) return false;
            ImageCodecInfo[] arr = ImageCodecInfo.GetImageEncoders();
            Type tp = arr.GetType();
            if (!iw.Indent(arr)) return false;
            //System.Drawing.Imaging.Encoder;
            //iw.WriteLine(string.Format("# <{0}>", "GetImageEncoders"));
            iw.WriteLine(string.Format("# <{0}>", tp.FullName));
            iw.WriteLine(string.Format("# Count: {0}", arr.Length));
            for (int i = 0; i < arr.Length; ++i) {
                ImageCodecInfo cur = arr[i];
                iw.WriteLine(string.Format("[{0}]:\t{1}\t # <{2}>", i, cur, cur.GetType().Name));
                iw.Indent(cur);
                iw.WriteLine(string.Format("Clsid:\t{0}", cur.Clsid));
                iw.WriteLine(string.Format("CodecName:\t{0}", cur.CodecName));
                iw.WriteLine(string.Format("DllName:\t{0}", cur.DllName));
                iw.WriteLine(string.Format("FilenameExtension:\t{0}", cur.FilenameExtension));
                iw.WriteLine(string.Format("FormatDescription:\t{0}", cur.FormatDescription));
                iw.WriteLine(string.Format("FormatID:\t{0}", cur.FormatID));
                iw.WriteLine(string.Format("MimeType:\t{0}", cur.MimeType));
                iw.WriteLine(string.Format("Version:\t{0}", cur.Version));
                // GetImageDecoders.
                try {
                    EncoderParameters encoderParameters = obj.GetEncoderParameterList(cur.Clsid);
                    iw.WriteLine(string.Format("GetEncoderParameterList():\t{0}", encoderParameters));
                    if (null != encoderParameters && null != encoderParameters.Param) {
                        iw.Indent(encoderParameters);
                        IndentedWriterMemberOptions options = IndentedWriterMemberOptions.AllowMethod;
                        IndentedWriterUtil.ForEachMember(iw, encoderParameters, null, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
                            //Debug.WriteLine(e.MemberName);
                        }, context);
                        iw.Unindent();
                    }
                } catch (Exception ex) {
                    Debug.WriteLine(cur.CodecName);
                    Debug.WriteLine(ex);
                    iw.WriteLine(string.Format("GetEncoderParameterList():\t#{0}", ex.Message));
                }
                // done.
                iw.Unindent();
            }
            iw.Unindent();
            return true;
        }

        /// <summary>
        /// 输出多行_Image.
        /// </summary>
        /// <param name="iw">带缩进输出者.</param>
        /// <param name="obj">object. Can be null.</param>
        /// <param name="context">State Object. Can be null.</param>
        /// <returns>返回是否成功输出.</returns>
        private static bool outl_Image(IIndentedWriter iw, Image obj, IndentedWriterContext context) {
            if (null == obj) return false;
            if (null == iw) return false;
            Type tp = obj.GetType();
            if (!iw.Indent(obj)) return false;
            iw.WriteLine(string.Format("# <{0}>", tp.FullName));
            // ShowCompression
            bool ShowCompression = true;
            if (ShowCompression) {
                //EncoderValue
                const int PropertyTagCompression = 0x103; // 0x103: PropertyTagCompression
                int compressionTagIndex = Array.IndexOf(obj.PropertyIdList, PropertyTagCompression);
                if (compressionTagIndex >= 0) {
                    PropertyItem compressionTag = obj.PropertyItems[compressionTagIndex];
                    int compression = BitConverter.ToInt16(compressionTag.Value, 0);
                    string name = null;
                    IMAGE_COMPRESSION_Map.TryGetValue(compression, out name);
                    iw.WriteLine("# Compression:\t{0}	# (0x{0:X}), {1}", compression, name);
                }
            }
            // body.
            IndentedWriterMemberOptions options = IndentedWriterMemberOptions.AllowMethod;
            IndentedWriterUtil.ForEachMember(iw, obj, tp, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
                //Debug.WriteLine(e.MemberName);
                if (false) {
                } else if (IndentedWriterUtil.StringComparer.Equals(e.MemberName, "PropertyItems")) {
                    if (!ShowBytes) {
                        e.IsCancel = true;
                    }
                } else if (IndentedWriterUtil.StringComparer.Equals(e.MemberName, "PropertyIdList")) {
                    e.IsCancel = true;
                }
            }, context);
            // PropertyIdList.
            int[] list = obj.PropertyIdList;
            if (null != list) {
                iw.WriteLine("PropertyIdList:\t{0}	# Length={1} (0x{1:X})", list, list.Length);
                iw.Indent(list);
                for (int i = 0; i < list.Length; ++i) {
                    int m = list[i];
                    string name = "";
                    PropertyIdMap.TryGetValue(m, out name);
                    iw.WriteLine("[{0}]:\t{1}\t# 0x{1:X}: {2}", i, m, name);
                }
                iw.Unindent();
            }
            // FrameDimensionsList
            const int frameIndex = 1;
            try {
                Guid[] frameDimensionsList = obj.FrameDimensionsList;
                if (null != frameDimensionsList && frameDimensionsList.Length > 0) {
                    foreach (Guid guid in frameDimensionsList) {
                        FrameDimension dimension = null; // new FrameDimension(guid);
                        string nameFrameDimension = null;
                        KeyValuePair<string, FrameDimension> found;
                        if (FrameDimensionMap.TryGetValue(guid, out found)) {
                            nameFrameDimension = found.Key;
                            dimension = found.Value;
                        } else {
                            dimension = new FrameDimension(guid);
                        }
                        iw.WriteLine("FrameDimension({0}, {1}):", guid, nameFrameDimension);
                        iw.Indent(guid);
                        try {
                            int n = obj.GetFrameCount(dimension);
                            iw.WriteLine("GetFrameCount():\t{0}", n);
                            if (frameIndex < n) {
                                iw.WriteLine("SelectActiveFrame({0}):", frameIndex);
                                iw.Indent(null);
                                try {
                                    obj.SelectActiveFrame(dimension, frameIndex);
                                    IndentedWriterUtil.ForEachMember(iw, obj, tp, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
                                        //Debug.WriteLine(e.MemberName);
                                        if (false) {
                                        } else if (IndentedWriterUtil.StringComparer.Equals(e.MemberName, "PropertyItems")) {
                                            if (!ShowBytes) {
                                                e.IsCancel = true;
                                            }
                                        } else if (IndentedWriterUtil.StringComparer.Equals(e.MemberName, "PropertyIdList")) {
                                            e.IsCancel = true;
                                        }
                                    }, context);
                                } catch (Exception ex2) {
                                    Debug.WriteLine(ex2.Message);
                                } finally {
                                    iw.Unindent();
                                }
                            }
                        } finally {
                            iw.Unindent();
                        }
                    }
                }
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
            // done.
            iw.Unindent();
            return true;
        }

        /// <summary>
        /// 输出多行_主函数_内部.
        /// </summary>
        /// <param name="iw">带缩进输出者.</param>
        /// <param name="obj">object. Can be null.</param>
        /// <param name="context">State Object. Can be null.</param>
        /// <returns>返回是否成功输出.</returns>
        private static bool outl_main_core(IIndentedWriter iw, object obj, IndentedWriterContext context) {
            if (null == iw) return false;
            IndentedWriterUtil.AddEnumType(typeof(System.Drawing.Imaging.Encoder));
            IndentedWriterUtil.AddEnumType(typeof(System.Drawing.Imaging.FrameDimension));
            IndentedWriterUtil.AddEnumType(typeof(System.Drawing.Imaging.ImageFormat));
            // body.
            iw.WriteLine("# zinfoimage");
            iw.WriteLine("Syntax: zinfoimage <FilePath>");
            iw.WriteLine("---");
            iw.WriteLine("FilePath:\t{0}", FilePath);
            if (string.IsNullOrEmpty(FilePath)) {
                iw.WriteLine("FilePath is empty!");
                return true;
            }
            // file.
            try {
                FileInfo fi = new FileInfo(FilePath);
                iw.WriteLine("FileInfo:");
                iw.Indent(fi);
                try {
                    if (ShowBytes) {
                        IndentedWriterMemberOptions options = IndentedWriterMemberOptions.AllowMethod;
                        IndentedWriterUtil.ForEachMember(iw, fi, null, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
                            //Debug.WriteLine(e.MemberName);
                        }, context);
                        // GetAccessControl
                        object fileSecurity = fi.GetAccessControl();
                        iw.WriteLine("GetAccessControl():");
                        iw.Indent(fileSecurity);
                        try {
                            IndentedWriterUtil.ForEachMember(iw, fileSecurity, null, options, delegate(object sender, IndentedWriterMemberEventArgs e) {
                                //Debug.WriteLine(e.MemberName);
                            }, context); // 太长, 必须在 SimpleTypeNames 里增加 `System.RuntimeType` .
                        } finally {
                            iw.Unindent();
                        }
                    } else {
                        iw.WriteLine("Length:\t{0}", fi.Length);
                    }
                } finally {
                    iw.Unindent();
                }
            } catch (Exception ex) {
                iw.WriteLine(ex);
            }
            // image.
            Image image = null;
            iw.WriteLine("Image:");
            try {
                image = Image.FromFile(FilePath);
            } catch (Exception ex) {
                iw.WriteLine(ex);
            }
            if (null == image) return true;
            //iw.WriteLine("Image.FromFile done.");
            outl_Image(iw, image, context);
            // GetImageEncoders.
            iw.WriteLine("ImageCodecInfo.GetImageEncoders():");
            outl_GetEncoderParameterList(iw, image, context);
            // Others.
            iw.WriteLine("Encoder:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(Encoder), context);
            iw.WriteLine("FrameDimension:"); IndentedWriterUtil.WriteTypeStatic(iw, typeof(FrameDimension), context);
            return true;
        }

        /// <summary>
        /// On ForEachMemberBegin
        /// </summary>
        /// <param name="iw"></param>
        /// <param name="owner"></param>
        /// <param name="tp"></param>
        /// <param name="options"></param>
        /// <param name="handle"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static bool OnForEachMemberBegin(IIndentedWriter iw, object owner, Type tp, IndentedWriterMemberOptions options, EventHandler<IndentedWriterMemberEventArgs> handle, IndentedWriterContext context) {
            Debug.WriteLine(tp);
            return true;
        }

        /// <summary>
        /// 输出多行_主函数. 不会枚举 颜色、画笔、刷子信息.
        /// </summary>
        /// <param name="iw">带缩进输出者.</param>
        /// <param name="obj">object. Can be null.</param>
        /// <param name="context">State Object. Can be null.</param>
        /// <returns>返回是否成功输出.</returns>
        public static bool outl_main(IIndentedWriter iw, object obj, IndentedWriterContext context) {
            if (null == context) context = new IndentedWriterContext();
            context.ForEachMemberBegin += OnForEachMemberBegin;
            return outl_main_core(iw, obj, context);
        }
    }
}
