﻿// Copyright 2014 Hugo Biarge
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Payroll.Integration.Feeds.Quartz
{
    using global::Quartz;
    using global::Quartz.Core;
    using global::Quartz.Impl;

    public class UnitySchedulerFactory : StdSchedulerFactory
    {
        private readonly UnityJobFactory unityJobFactory;

        public UnitySchedulerFactory(UnityJobFactory unityJobFactory)
        {
            this.unityJobFactory = unityJobFactory;
        }

        protected override IScheduler Instantiate(QuartzSchedulerResources rsrcs, QuartzScheduler qs)
        {
            qs.JobFactory = this.unityJobFactory;
            return base.Instantiate(rsrcs, qs);
        }
    }
}
