makecert.exe ^
-n "CN=IMCARoot" ^
-r ^
-pe ^
-a sha512 ^
-len 4096 ^
-cy authority ^
-sv IMCA.pvk ^
IMCARoot.cer
 
pvk2pfx.exe ^
-pvk IMCA.pvk ^
-spc IMCARoot.cer ^
-pfx IM.pfx ^
-po 1A2B3C4F