import { createContext, useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

// Mock user interface
interface MockUser {
  id: string;
  email: string;
  fullName: string;
  phone?: string;
  role: "doctor" | "nurse" | "admin";
}

interface AuthContextType {
  user: MockUser | null;
  session: { user: MockUser } | null;
  userRole: string | null;
  loading: boolean;
  signOut: () => Promise<void>;
  signIn: (email: string, password: string) => Promise<{ success: boolean; error?: string }>;
  signUp: (email: string, password: string, fullName: string, phone: string, role: "doctor" | "nurse" | "admin") => Promise<{ success: boolean; error?: string }>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

// Storage key for fake auth
const STORAGE_KEY = "fake_auth_session";

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<MockUser | null>(null);
  const [session, setSession] = useState<{ user: MockUser } | null>(null);
  const [userRole, setUserRole] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    // Check for existing session in localStorage
    const storedSession = localStorage.getItem(STORAGE_KEY);
    if (storedSession) {
      try {
        const sessionData = JSON.parse(storedSession);
        setUser(sessionData.user);
        setSession({ user: sessionData.user });
        setUserRole(sessionData.user.role);
      } catch (error) {
        console.error("Error parsing stored session:", error);
        localStorage.removeItem(STORAGE_KEY);
      }
    }
    setLoading(false);
  }, []);

  const signIn = async (email: string, password: string): Promise<{ success: boolean; error?: string }> => {
    // Simulate API delay
    await new Promise(resolve => setTimeout(resolve, 500));
    
    // Check localStorage for registered users
    const users = JSON.parse(localStorage.getItem("fake_users") || "[]");
    const foundUser = users.find((u: MockUser & { password: string }) => u.email === email);
    
    if (!foundUser) {
      return { success: false, error: "Invalid email or password" };
    }
    
    if (foundUser.password !== password) {
      return { success: false, error: "Invalid email or password" };
    }
    
    // Create session
    const { password: _, ...userWithoutPassword } = foundUser;
    const sessionData = { user: userWithoutPassword };
    
    localStorage.setItem(STORAGE_KEY, JSON.stringify(sessionData));
    setUser(userWithoutPassword);
    setSession(sessionData);
    setUserRole(userWithoutPassword.role);
    
    return { success: true };
  };

  const signUp = async (
    email: string,
    password: string,
    fullName: string,
    phone: string,
    role: "doctor" | "nurse" | "admin"
  ): Promise<{ success: boolean; error?: string }> => {
    // Simulate API delay
    await new Promise(resolve => setTimeout(resolve, 500));
    
    // Check if user already exists
    const users = JSON.parse(localStorage.getItem("fake_users") || "[]");
    if (users.find((u: MockUser) => u.email === email)) {
      return { success: false, error: "This email is already registered" };
    }
    
    // Create new user
    const newUser: MockUser & { password: string } = {
      id: `user_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`,
      email,
      fullName,
      phone,
      role,
      password, // In real app, this would be hashed
    };
    
    // Save user
    users.push(newUser);
    localStorage.setItem("fake_users", JSON.stringify(users));
    
    // Auto sign in
    const { password: _, ...userWithoutPassword } = newUser;
    const sessionData = { user: userWithoutPassword };
    
    localStorage.setItem(STORAGE_KEY, JSON.stringify(sessionData));
    setUser(userWithoutPassword);
    setSession(sessionData);
    setUserRole(userWithoutPassword.role);
    
    return { success: true };
  };

  const signOut = async () => {
    localStorage.removeItem(STORAGE_KEY);
    setUser(null);
    setSession(null);
    setUserRole(null);
    navigate("/auth");
  };

  return (
    <AuthContext.Provider value={{ user, session, userRole, loading, signOut, signIn, signUp }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
}
