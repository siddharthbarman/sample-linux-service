sudo cp 
sudo cp SampleLinuxService.service /etc/systemd/system/
sudo systemctl daemon-reload
sudo systemctl status SampleLinuxService
sudo systemctl start SampleLinuxService
 sudo journalctl -e -u SampleLinuxService # see the latest 100 lines