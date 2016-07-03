echo off
title Set Power options
cls
echo Setting power settings
echo.
powercfg -x -monitor-timeout-ac 30
echo Set Monitor sleep time to    30 min.

powercfg -x -disk-timeout-ac 0
echo Set Hard Disk timeout        Never.
powercfg -x -standby-timeout-ac 0
echo Sleep timeout                Never.
powercfg -x -hibernate-timeout-ac 0
echo Hibernate timeout            Never.
echo.
Powercfg -setacvalueindex SCHEME_MIN SUB_SLEEP HYBRIDSLEEP 0
echo Hybrid Sleep                 OFF
echo.