import { PatientHeader } from "./PatientHeader";
import { DiagnosticResults } from "./DiagnosticResults";
import { VitalSigns } from "./VitalSigns";
import { SymptomsAnalysis } from "./SymptomsAnalysis";
import { TreatmentRecommendations } from "./TreatmentRecommendations";
import { PhysicianNotes } from "./PhysicianNotes";
import { Alerts } from "./Alerts";

// Sample data - in real app this would come from props or API
const sampleData = {
  patient: {
    fullName: "Sarah Johnson",
    medicalId: "MRN-789123",
    age: 34,
    gender: "Female",
    dateOfExamination: "2024-07-25",
    maritalStatus: "Pregnant (28 weeks)",
    specialConditions: ["Pregnancy", "Gestational Diabetes"]
  },
  severity: "medium" as const,
  chiefComplaint: {
    complaint: "Severe sore throat and difficulty swallowing",
    duration: "3 days",
    bodySystem: "Respiratory"
  },
  primaryDiagnosis: {
    condition: "Acute Streptococcal Tonsillitis",
    probability: 92,
    isPrimary: true
  },
  differentialDiagnoses: [
    { condition: "Viral Pharyngitis", probability: 68 },
    { condition: "Peritonsillar Abscess", probability: 23 },
    { condition: "Mononucleosis", probability: 15 }
  ],
  excludedDiagnoses: ["COVID-19", "Diphtheria", "Epiglottitis"],
  reasonsForExclusion: {
    "COVID-19": "Negative rapid antigen test, no fever or respiratory symptoms",
    "Diphtheria": "Patient is vaccinated, no pseudomembrane formation",
    "Epiglottitis": "No drooling, able to swallow liquids, no stridor"
  },
  symptoms: [
    { name: "Sore throat", present: true, severity: "severe" as const, duration: "3 days" },
    { name: "Difficulty swallowing", present: true, severity: "moderate" as const },
    { name: "Swollen lymph nodes", present: true, severity: "mild" as const },
    { name: "Fever", present: true, severity: "moderate" as const },
    { name: "Cough", present: false },
    { name: "Runny nose", present: false },
    { name: "Headache", present: false },
    { name: "Nausea", present: false }
  ],
  vitals: {
    temperature: { value: 101.2, unit: "°F" },
    pulse: { value: 88, unit: "bpm" },
    bloodPressure: { systolic: 118, diastolic: 76 },
    respiratoryRate: { value: 16, unit: "/min" },
    oxygenSaturation: { value: 98, unit: "%" }
  },
  alerts: [
    {
      type: "warning" as const,
      message: "Patient is pregnant - avoid certain antibiotics",
      priority: "high" as const
    },
    {
      type: "info" as const,
      message: "Gestational diabetes - monitor blood glucose if prescribing steroids",
      priority: "medium" as const
    }
  ]
};

export const MedicalDashboard = () => {
  return (
    <div className="min-h-screen bg-background">
      <div className="container mx-auto p-4 space-y-6">
        {/* Patient Header */}
        <PatientHeader 
          patient={sampleData.patient} 
          severity={sampleData.severity} 
        />

        {/* Alerts */}
        <Alerts alerts={sampleData.alerts} />

        {/* Chief Complaint & Symptoms */}
        <SymptomsAnalysis 
          chiefComplaint={sampleData.chiefComplaint}
          symptoms={sampleData.symptoms}
        />

        {/* Diagnostic Results */}
        <DiagnosticResults
          primaryDiagnosis={sampleData.primaryDiagnosis}
          differentialDiagnoses={sampleData.differentialDiagnoses}
          excludedDiagnoses={sampleData.excludedDiagnoses}
          reasonsForExclusion={sampleData.reasonsForExclusion}
        />

        {/* Vital Signs */}
        <VitalSigns vitals={sampleData.vitals} />

        {/* Treatment Recommendations */}
        <TreatmentRecommendations />

        {/* Physician Notes */}
        <PhysicianNotes />
      </div>
    </div>
  );
};