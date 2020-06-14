using System;
using System.Collections.Generic;
using CommandLine;
using ZoomApi;
// ReSharper disable ClassNeverInstantiated.Local

namespace Zoom
{
    class Program
    {
        [Verb("users", HelpText = "List zoom users")]
        private class ListUsersOptions : BaseOptions
        {
        }
        [Verb("meetings", HelpText = "List zoom meetings")]
        private class ListMeetingsOptions : BaseOptions
        {
        }

        [Verb("participants", HelpText = "List zoom meetings")]
        private class ListParticipantsOptions : BaseOptions
        {
            [Option('m', "meetingId", Required = true, HelpText = "Set the Zoom API Key")]
            public string MeetingId { get; set; }
        }

        private class BaseOptions
        {
            [Option('k', "apikey", Required = true, HelpText = "Set the Zoom API Key")]
            public string ApiKey { get; set; }
            [Option('s', "secret", Required = true, HelpText = "Set the Zoom API Secret")]
            public string ApiSecret { get; set; }

        }
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<ListUsersOptions, ListMeetingsOptions, ListParticipantsOptions>(args)
               .MapResult(
                   (ListUsersOptions o) => ListUsers(o),
            (ListMeetingsOptions o) => ListMeetings(o),
            (ListParticipantsOptions o) => ListParticipants(o),
                   errs => 1);
        }


        private static int ListParticipants(ListParticipantsOptions opts)
        {
            var zoomService = new ZoomService(opts.ApiKey, opts.ApiSecret);
            var participants = zoomService.ListParticipants(opts.MeetingId).ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (var participant in participants)
            {
                Console.WriteLine($"Id: {participant.UserId}; UserName: {participant.UserName}");
            }

            return 0;
        }

        private static int ListMeetings(ListMeetingsOptions opts)
        {
            var zoomService = new ZoomService(opts.ApiKey, opts.ApiSecret);
            var allMeetings = new List<Meeting>();
            var users = zoomService.ListUsers().ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (var user in users)
            {
                var meetings = zoomService.ListMeetings(user.Id, MeetingState.Scheduled).ConfigureAwait(false).GetAwaiter().GetResult();
                allMeetings.AddRange(meetings);
            }

            foreach (var meeting in allMeetings)
            {
                Console.WriteLine($"Id: {meeting.Id}; Topic: {meeting.Topic}; Host: {meeting.HostId}; Start: {meeting.StartTime}; JoinUrl: {meeting.JoinUrl}");
            }
            return 0;
        }

        private static int ListUsers(ListUsersOptions opts)
        {
            var zoomService = new ZoomService(opts.ApiKey, opts.ApiSecret);
            var users = zoomService.ListUsers().ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (var user in users)
            {
                Console.WriteLine($"Id: {user.Id}; First name: {user.FirstName}; Last name: {user.LastName}; eMail: {user.Email}");
            }
            return 0;
        }
    }
}
