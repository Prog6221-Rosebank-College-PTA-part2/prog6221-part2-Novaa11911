using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace Professor_Bot_GUI
{
    public class ResponsesService
    {
        private Random rand = new Random();
        private SpeechSynthesizer speaker;

        private Dictionary<string, string> memory = new Dictionary<string, string>();
        private Dictionary<string, int> topicCount = new Dictionary<string, int>();

        public ResponsesService()
        {
            speaker = new SpeechSynthesizer();
            speaker.Volume = 100;
            speaker.Rate = 0;
        }


        private void TrackTopic(string topic)
        {
            if (!topicCount.ContainsKey(topic))
                topicCount[topic] = 0;

            topicCount[topic]++;
        }


        private string HandleRepeat(string key, string[] advice, string label)
        {
            if (!topicCount.ContainsKey(key))
                topicCount[key] = 0;

            if (topicCount[key] > 1)
            {
                return $"You already asked about {label}. Reminder: {GetRandom(advice)}";
            }

            return GetRandom(advice);
        }


        public string GetResponse(string input, string name)
        {
            if (string.IsNullOrWhiteSpace(input))
                return GetRandom(emptyResponses);

            input = input.ToLower();
            memory["name"] = name;

            string mood = DetectMood(input);

            if (mood == "happy")
                return SpeakAndReturn(GetRandom(happyResponses));

            if (mood == "sad")
                return SpeakAndReturn(GetRandom(sadResponses));

            if (mood == "angry")
                return SpeakAndReturn(GetRandom(angryResponses));

   
            if (input.Contains("privacy"))
            {
                memory["topic"] = "privacy";
                TrackTopic("privacy");
                return SpeakAndReturn(HandleRepeat("privacy", privacyAdvice, "privacy"));
            }

            if (input.Contains("password"))
            {
                memory["topic"] = "password security";
                TrackTopic("password");
                return SpeakAndReturn(HandleRepeat("password", passwordAdvice, "password security"));
            }

            if (input.Contains("vpn") || input.Contains("2fa") || input.Contains("authentication"))
            {
                memory["topic"] = "vpn security";
                TrackTopic("vpn");
                return SpeakAndReturn(HandleRepeat("vpn", vpnAdvice, "VPN & authentication"));
            }

            if (input.Contains("facebook") || input.Contains("instagram") || input.Contains("tiktok"))
            {
                memory["social"] = input;
                return SpeakAndReturn("I will remember your social media interests.");
            }

            if (input.Contains("recall") || input.Contains("remember"))
                return SpeakAndReturn(RecallMemory());

            if (input.Contains("advice"))
            {
                if (memory.ContainsKey("name") && memory.ContainsKey("topic"))
                    return SpeakAndReturn($"{memory["name"]}, stay careful with {memory["topic"]} online.");

                return SpeakAndReturn("I need more information first.");
            }

            if (input.Contains("hello") || input.Contains("hi") || input.Contains("hey"))
                return SpeakAndReturn(GetRandom(greetings));

            if (input == "bye" || input == "exit" || input == "goodbye")
                return SpeakAndReturn($"Goodbye {name}! {GetRandom(goodbyeResponses)}");

            return SpeakAndReturn(GetRandom(unknownResponses));
        }

        private string DetectMood(string input)
        {
            if (input.Contains("sad") || input.Contains("depressed") || input.Contains("upset"))
                return "sad";

            if (input.Contains("angry") || input.Contains("mad") || input.Contains("frustrated"))
                return "angry";

            if (input.Contains("happy") || input.Contains("good") || input.Contains("great"))
                return "happy";

            return "neutral";
        }


        private string RecallMemory()
        {
            string response = "";

            foreach (var item in memory)
                response += $"{item.Key}: {item.Value}\n";

            return string.IsNullOrWhiteSpace(response)
                ? "I do not remember anything yet."
                : response;
        }

        private string SpeakAndReturn(string message)
        {
            Speak(message);
            return message;
        }

        private void Speak(string message)
        {
            speaker.SpeakAsyncCancelAll();
            speaker.SpeakAsync(message);
        }

        public void SpeakMessage(string message)
        {
            Speak(message);
        }

        private string GetRandom(string[] arr)
        {
            return arr[rand.Next(arr.Length)];
        }

        private string[] happyResponses = {
            "Glad you're feeling good .. Stay safe online!", "Nice! Keep that positive energy going.",
            "Good to hear! Let’s keep your accounts secure too." };

        private string[] sadResponses = {
            "I'm here for you.", "Sorry you're feeling that way.",
            "Take your time." };
        private string[] angryResponses = { "I understand you're frustrated.",
            "No stress — I’ll help you step by step.", "Let’s slow down." };
        private string[] greetings = { "Hello!",
            "Hi there!",
            "Welcome!" };
        private string[] emptyResponses = { "Please type something.",
            "Input is empty.", 
            "Try again." };
        private string[] unknownResponses = { "I didn't understand that.",
            "Try rephrasing.",
            "Not sure what you mean." };

        private string[] passwordAdvice = { "Use strong passwords.", 
            "Enable 2FA.", 
            "Never reuse passwords." };
        private string[] phishingAdvice = { "Never click suspicious links.",
            "Check email carefully.", 
            "Be cautious of urgency." };
        private string[] vpnAdvice = { "Use VPN on public Wi-Fi.",
            "VPN encrypts traffic.",
            "Choose trusted providers." };
        private string[] privacyAdvice = { "Review privacy settings.",
            "Avoid sharing sensitive info.",
            "Protect your identity." };
        private string[] goodbyeResponses = { "Goodbye!", "Stay safe online.",
            "See you later!" };
    }
}