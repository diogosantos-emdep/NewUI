NET USE @LPT: "\\@SelectedPrinterIp\@SharedPrinterName"
COPY %1 @LPT   
NET USE @LPT /DELETE       