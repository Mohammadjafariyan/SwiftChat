import {useEffect} from "react";

const useScript = url => {

    if(process.env.NODE_ENV === "development") {
        url = 'http://localhost:60518' +url
    }
    
    useEffect(() => {
        const script = document.createElement('script');

        script.src = url;
        script.async = false;

        document.body.appendChild(script);

        return () => {
            document.body.removeChild(script);
        }
    }, [url]);
};

export default useScript;

