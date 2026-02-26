var HapticPlugin = {
    Vibrate: function(ms) {
        if (navigator.vibrate) {
            navigator.vibrate(ms);
        } else {
            console.log("Vibration not supported in this browser.");
        }
    }
};

mergeInto(LibraryManager.library, HapticPlugin);