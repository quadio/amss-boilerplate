@ECHO OFF
for /f "tokens=2 delims=:." %%x in ('chcp') do set cp=%%x
chcp 65001>nul
ECHO START Install
CMD /u /c Setup.CMD > Setup.log
ECHO END Install, SEE Setup.log
chcp %cp%>nul