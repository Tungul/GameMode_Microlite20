//@name Debugging
//@author Meshiest
//@description A simple functiont that snifs out all the errors in all of the .cs files!

function m20_debug()
{
   %p="Add-ons/Gamemode_Microlite20/*.cs";
   %m=-1;
   echo("M20: --Starting Debug-- "@getDateTime());
   for(%f=findFirstFile(%p);%f!$="";%f=findNextFile(%p))
   {
      %c=compile(%f);
      echo("M20: "@(!%c?"\c2":"\c0")@%f);
      if(!%c)
         %e[%m++]=%f;
   }
   echo("M20: Problem Files:");
   for(%i=0;%i<=%m;%i++)
      echo(%e[%m]);
   echo("M20: Debug Complete!");
}