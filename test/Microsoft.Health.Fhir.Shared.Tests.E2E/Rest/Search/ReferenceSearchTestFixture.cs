﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Hl7.Fhir.Model;
using Microsoft.Health.Fhir.Client;
using Microsoft.Health.Fhir.Tests.Common.FixtureParameters;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.Health.Fhir.Tests.E2E.Rest.Search
{
    public class ReferenceSearchTestFixture : HttpIntegrationTestFixture
    {
        public ReferenceSearchTestFixture(DataStore dataStore, Format format, TestFhirServerFactory testFhirServerFactory)
            : base(dataStore, format, testFhirServerFactory)
        {
        }

        public IReadOnlyList<Patient> Patients { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            // Prepare the resources used for string search tests.
            await TestFhirClient.DeleteAllResources(ResourceType.Patient);

            Patients = await TestFhirClient.CreateResourcesAsync<Patient>(
                p => p.ManagingOrganization = new ResourceReference("Organization/123"),
                p => p.ManagingOrganization = new ResourceReference("Organization/abc"),
                p => p.ManagingOrganization = new ResourceReference("ijk"), // type not specified, but known constrained to be Organization
                p => p.GeneralPractitioner = new List<ResourceReference> { new ResourceReference("Practitioner/p1") },
                p => p.GeneralPractitioner = new List<ResourceReference> { new ResourceReference("p2") }); // type not specified and not known because it could be Practitioner, Organization, or PractitionerRole
        }
    }
}
