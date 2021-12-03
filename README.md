# mpp-solar-poller
Mono app that read from mpp-solar usb port using a raspberry pi zero w


App based on this repo : https://github.com/ned-kelly/docker-voltronic-homeassistant
And rewritten to my needs


from your raspberry pi zero, 
msbuild -restore

Then sudo mono MppSolarPoller.exe from the build output directory
