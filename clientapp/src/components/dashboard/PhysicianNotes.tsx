import { useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import { Badge } from "@/components/ui/badge";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { FileText, Save, X, Check } from "lucide-react";
import { toast } from "sonner";

interface Note {
  id: string;
  text: string;
  timestamp: string;
  physician: string;
  type: "assessment" | "plan" | "observation" | "modification";
}

export const PhysicianNotes = () => {
  const [notes, setNotes] = useState<Note[]>([
    {
      id: "1",
      text: "Patient presents with classic signs of streptococcal tonsillitis. Given pregnancy status, selected amoxicillin as first-line treatment.",
      timestamp: "2024-07-25 10:30 AM",
      physician: "Dr. Smith",
      type: "assessment"
    }
  ]);

  const [newNote, setNewNote] = useState("");
  const [noteType, setNoteType] = useState<"assessment" | "plan" | "observation" | "modification">("observation");
  const [aiRecommendationStatus, setAiRecommendationStatus] = useState<"accepted" | "modified" | "rejected" | null>(null);
  const [rejectionReason, setRejectionReason] = useState("");

  const handleSaveNote = () => {
    if (!newNote.trim()) {
      toast("Please enter a note before saving");
      return;
    }

    const note: Note = {
      id: Date.now().toString(),
      text: newNote,
      timestamp: new Date().toLocaleString(),
      physician: "Current Physician",
      type: noteType
    };

    setNotes([note, ...notes]);
    setNewNote("");
    toast("Note saved successfully");
  };

  const handleAiRecommendationAction = (action: "accepted" | "modified" | "rejected", reason?: string) => {
    setAiRecommendationStatus(action);
    if (action === "rejected" && reason) {
      setRejectionReason(reason);
      const note: Note = {
        id: Date.now().toString(),
        text: `AI recommendation rejected. Reason: ${reason}`,
        timestamp: new Date().toLocaleString(),
        physician: "Current Physician",
        type: "modification"
      };
      setNotes([note, ...notes]);
    }
    toast(`AI recommendation ${action}`);
  };

  const getTypeColor = (type: string) => {
    switch (type) {
      case "assessment": return "bg-primary text-primary-foreground";
      case "plan": return "bg-medical-success text-medical-success-foreground";
      case "observation": return "bg-medical-info text-medical-info-foreground";
      case "modification": return "bg-medical-warning text-medical-warning-foreground";
      default: return "bg-muted text-muted-foreground";
    }
  };

  return (
    <div className="grid gap-6 lg:grid-cols-3">
      {/* AI Recommendation Status */}
      <Card className="shadow-card">
        <CardHeader>
          <CardTitle className="flex items-center text-foreground">
            <FileText className="h-5 w-5 mr-2 text-primary" />
            AI Recommendation Status
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <div className="p-3 border border-border rounded-lg bg-muted/30">
              <h4 className="font-medium text-foreground mb-2">Saleem AI Recommendation</h4>
              <p className="text-sm text-muted-foreground mb-3">
                Prescribe Amoxicillin 500mg TID for 10 days. Supportive care with fluids and rest.
              </p>
              
              {!aiRecommendationStatus ? (
                <div className="flex flex-col space-y-2">
                  <Button 
                    size="sm" 
                    className="bg-medical-success text-medical-success-foreground hover:bg-medical-success/90"
                    onClick={() => handleAiRecommendationAction("accepted")}
                  >
                    <Check className="h-4 w-4 mr-1" />
                    Accept
                  </Button>
                  <Button 
                    size="sm" 
                    variant="outline"
                    onClick={() => handleAiRecommendationAction("modified")}
                  >
                    <FileText className="h-4 w-4 mr-1" />
                    Accept with Modifications
                  </Button>
                  <Button 
                    size="sm" 
                    variant="outline"
                    className="text-medical-danger border-medical-danger hover:bg-medical-danger hover:text-medical-danger-foreground"
                    onClick={() => {
                      const reason = prompt("Please provide reason for rejection:");
                      if (reason) handleAiRecommendationAction("rejected", reason);
                    }}
                  >
                    <X className="h-4 w-4 mr-1" />
                    Reject
                  </Button>
                </div>
              ) : (
                <Badge className={`${
                  aiRecommendationStatus === "accepted" 
                    ? "bg-medical-success text-medical-success-foreground"
                    : aiRecommendationStatus === "modified"
                    ? "bg-medical-warning text-medical-warning-foreground"
                    : "bg-medical-danger text-medical-danger-foreground"
                }`}>
                  {aiRecommendationStatus === "accepted" && "Accepted"}
                  {aiRecommendationStatus === "modified" && "Modified"}
                  {aiRecommendationStatus === "rejected" && "Rejected"}
                </Badge>
              )}
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Add New Note */}
      <Card className="shadow-card">
        <CardHeader>
          <CardTitle className="flex items-center text-foreground">
            <FileText className="h-5 w-5 mr-2 text-primary" />
            Add Physician Note
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <Select value={noteType} onValueChange={(value: any) => setNoteType(value)}>
              <SelectTrigger>
                <SelectValue placeholder="Select note type" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="assessment">Assessment</SelectItem>
                <SelectItem value="plan">Treatment Plan</SelectItem>
                <SelectItem value="observation">Clinical Observation</SelectItem>
                <SelectItem value="modification">AI Modification</SelectItem>
              </SelectContent>
            </Select>
            
            <Textarea
              placeholder="Enter your clinical notes here..."
              value={newNote}
              onChange={(e) => setNewNote(e.target.value)}
              className="min-h-[120px]"
            />
            
            <Button 
              onClick={handleSaveNote}
              className="w-full bg-primary text-primary-foreground hover:bg-primary/90"
            >
              <Save className="h-4 w-4 mr-2" />
              Save Note
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Notes History */}
      <Card className="shadow-card">
        <CardHeader>
          <CardTitle className="flex items-center text-foreground">
            <FileText className="h-5 w-5 mr-2 text-primary" />
            Notes History
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-3 max-h-[400px] overflow-y-auto">
            {notes.length === 0 ? (
              <p className="text-muted-foreground text-center py-4">No notes available</p>
            ) : (
              notes.map((note) => (
                <div key={note.id} className="p-3 border border-border rounded-lg bg-muted/30">
                  <div className="flex items-center justify-between mb-2">
                    <Badge className={`text-xs ${getTypeColor(note.type)}`}>
                      {note.type}
                    </Badge>
                    <span className="text-xs text-muted-foreground">{note.timestamp}</span>
                  </div>
                  <p className="text-sm text-foreground mb-1">{note.text}</p>
                  <p className="text-xs text-muted-foreground">- {note.physician}</p>
                </div>
              ))
            )}
          </div>
        </CardContent>
      </Card>
    </div>
  );
};