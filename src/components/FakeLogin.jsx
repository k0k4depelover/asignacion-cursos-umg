import { useState } from "react";
import '../styles/FakeLogin.css'

export default function FakeLogin({onLogin}){
    const [email, setEmail] = useState("eaguila2@umg.edu.gt");
    const [password, setPassword] = useState("password");
    const [loading, setLoading] = useState(false);

    const handleSubmit = (e) =>{
        e.preventDefault();
        setLoading(true);
        setTimeout( () =>{
            e.preventDefault;
            setLoading(true);
            onLogin({email, name: "Esduardo del Aguila"});
        }, 800)
    };

    return(
        <form onSubmit={handleSubmit} className="login-container">
            <h2 className="login-title">
                Iniciar Sesión
            </h2>

            <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                style={{ display: "block", width: "100%", marginBottom: 10 }}
                className="login-input"
            />
            <input 
                type="password" 
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                style={{display: "block", width: '100%', marginBottom:10}}
                className="login-input"    
            />

            <button type="submit" disabled={loading} className= "login-button" >
                {loading ? "Ingresando... ": "Entrar"}
                
            </button>

        </form>

    );

}